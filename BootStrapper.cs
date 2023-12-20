using System;
using System.Diagnostics;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Simple.Rest;
using VendingMachine.DataFile;
using VendingMachine.Services;
using VendingMachine.ViewModels;

namespace VendingMachine
{
    public static class BootStrapper
    {
        private static ILifetimeScope _rootScope;
        private static IMainViewModel _chromeViewModel;

        public static IViewModel RootVisual
        {
            get
            {
                if (_rootScope == null) Start();

                _chromeViewModel = _rootScope.Resolve<IMainViewModel>();
                return _chromeViewModel;
            }
        }

        public static void Start()
        {
            if (_rootScope != null) return;
            var builder = new ContainerBuilder();
            var assemblies = new[] {Assembly.GetExecutingAssembly()};

            builder.RegisterType<RestClient>()
                .SingleInstance()
                .AsImplementedInterfaces();

            builder.RegisterType<ApiService>()
              .SingleInstance()
              .AsImplementedInterfaces();


            builder.RegisterType<UartService>()
              .SingleInstance()
              .AsImplementedInterfaces();

            builder.RegisterType<LocalDBContext>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(IService).IsAssignableFrom(t))
                .SingleInstance()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(IViewModel).IsAssignableFrom(t) && !typeof(ITransientViewModel).IsAssignableFrom(t))
                .AsImplementedInterfaces();

            // several view model instances are transitory and created on the fly, if these are tracked by the container then they
            // won't be disposed of in a timely manner

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(IViewModel).IsAssignableFrom(t))
                .Where(t =>
                {
                    var isAssignable = typeof(ITransientViewModel).IsAssignableFrom(t);
                    if (isAssignable) Debug.WriteLine("Transient view model - " + t.Name);

                    return isAssignable;
                })
                .AsImplementedInterfaces()
                .ExternallyOwned();

            _rootScope = builder.Build();
        }

        public static void Stop()
        {
            _rootScope.Dispose();
        }

        public static T Resolve<T>()
        {
            if (_rootScope == null) throw new Exception("Bootstrapper hasn't been started!");

            return _rootScope.Resolve<T>(new Parameter[0]);
        }

        public static T Resolve<T>(Parameter[] parameters)
        {
            if (_rootScope == null) throw new Exception("Bootstrapper hasn't been started!");

            return _rootScope.Resolve<T>(parameters);
        }
    }
}