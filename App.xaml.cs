using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Autofac;
using Autofac.Core;
using NLog;
using VendingMachine.DataFile;
using VendingMachine.Extensions;
using VendingMachine.Helpers;
using VendingMachine.Models;
using VendingMachine.Resources.Views;
using VendingMachine.Services;
using VendingMachine.ViewModels;
using Duration = VendingMachine.Services.Duration;
using ObservableExtensions = VendingMachine.Extensions.ObservableExtensions;

namespace VendingMachine
{
    public partial class App
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly CompositeDisposable _disposable;

        public App()
        {
#if DEBUG
            LogHelper.ReconfigureLoggerToLevel(LogLevel.Debug);
#endif
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Current.DispatcherUnhandledException += DispatcherOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            _disposable = new CompositeDisposable();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
        //    using (var dataContext = new LocalDBContext())
        //    {
        //        dataContext.Products.Add(new Product() { Id = Guid.NewGuid() });
        //        dataContext.SaveChanges();

        //        foreach (var cat in dataContext.Products.ToList())
        //        {
        //            Console.WriteLine($"CategoryId= {cat.Id}");
        //        }

        //        Console.ReadLine();
        //    }

            using (Duration.Measure(Logger, "OnStartup - " + GetType()
                .Name))
            {
                Logger.Info("Starting");

                var dispatcherThreadInfo =
                    $"Dispatcher managed thread identifier = {Thread.CurrentThread.ManagedThreadId}";
                Debug.WriteLine(dispatcherThreadInfo);
                Logger.Info(dispatcherThreadInfo);

                Logger.Info($"WPF rendering capability (tier) = {RenderCapability.Tier / 0x10000}");
                RenderCapability.TierChanged += (s, a) =>
                    Logger.Info($"WPF rendering capability (tier) = {RenderCapability.Tier / 0x10000}");

                base.OnStartup(e);

                BootStrapper.Start();
                _disposable.Add(Disposable.Create(BootStrapper.Stop));

                var messageService = BootStrapper.Resolve<IMessageService>();
                var schedulerService = BootStrapper.Resolve<ISchedulerService>();

                ObservableExtensions.GestureService = BootStrapper.Resolve<IGestureService>();

                var window = new MainWindow(messageService, schedulerService);

                // The window has to be created before the root visual - all to do with the idling service initialising correctly...
                window.DataContext = BootStrapper.RootVisual;

                window.Closed += (s, a) => HandleWindowClose();
                Current.Exit += (s, a) => HandleExit();

                window.Show();

                if (Logger.IsInfoEnabled)
                    ObserveHeartbeat(schedulerService)
                        .DisposeWith(_disposable);

#if DEBUG
                ObserveUiFreeze()
                    .DisposeWith(_disposable);
#endif
                Logger.Info("Started");
            }
        }

        private void HandleWindowClose()
        {
            Logger.Info("Window closed");
            _disposable.Dispose();
        }

        private static void HandleExit()
        {
            Logger.Info("Bye Bye!");
            LogManager.Flush();
        }


        private static IDisposable ObserveHeartbeat(ISchedulerService schedulerService)
        {
            var dianosticsService = BootStrapper.Resolve<IDiagnosticsService>();

            return BootStrapper.Resolve<IHeartbeatService>()
                .Listen
                .SelectMany(x => dianosticsService.Memory.Take(1), (x, y) => y)
                .SelectMany(x => dianosticsService.Cpu.Take(1), (x, y) => new Tuple<Memory, int>(x, y))
                .SafeSubscribe(x =>
                {
                    var message =
                        $"Heartbeat (Memory={x.Item1.WorkingSetPrivateAsString()}, CPU={x.Item2}%)";

                    Debug.WriteLine(message);
                    Logger.Info(message);
                }, schedulerService.Dispatcher);
        }

        private static IDisposable ObserveUiFreeze()
        {
            var timer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = Constants.UI.Diagnostics.UiFreezeTimer
            };

            var previous = DateTime.Now;
            timer.Tick += (sender, args) =>
            {
                var current = DateTime.Now;
                var delta = current - previous;
                previous = current;

                if (delta > Constants.UI.Diagnostics.UiFreeze)
                    Debug.WriteLine($"UI Freeze = {delta.TotalMilliseconds} ms");
            };

            timer.Start();
            return Disposable.Create(timer.Stop);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Logger.Info("Unhandled app domain exception");
            HandleException(args.ExceptionObject as Exception);
        }

        private static void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            Logger.Info("Unhandled dispatcher thread exception");
            args.Handled = true;

            HandleException(args.Exception);
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args)
        {
            Logger.Info("Unhandled task exception");
            args.SetObserved();

            HandleException(args.Exception.GetBaseException());
        }

        private static void HandleException(Exception exception)
        {
            Logger.Error(exception);

            var messageService = BootStrapper.Resolve<IMessageService>();
            var schedulerService = BootStrapper.Resolve<ISchedulerService>();

            schedulerService.Dispatcher.Schedule(() =>
            {
                var parameters = new Parameter[]
                {
                    new NamedParameter("exception", exception)
                };

                var viewModel = BootStrapper.Resolve<IExceptionViewModel>(parameters);

                viewModel.Closed
                    .Take(1)
                    .Subscribe(x =>
                    {
                        viewModel.Dispose();

                        // Force all other potential exceptions to be realized
                        // from the Finalizer thread to surface to the UI
                        GC.Collect(2, GCCollectionMode.Forced);
                        GC.WaitForPendingFinalizers();
                    });

                messageService.Post(Constants.UI.ExceptionTitle, viewModel);
            });
        }
    }
}