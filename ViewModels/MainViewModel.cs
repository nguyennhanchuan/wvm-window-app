using System;
using System.Linq;
using System.Reactive.Disposables;
using VendingMachine.Commands;
using VendingMachine.DataFile;
using VendingMachine.Extensions;
using VendingMachine.Navigation;
using VendingMachine.Services;

namespace VendingMachine.ViewModels
{
    public sealed class MainViewModel : BaseViewModel, IMainViewModel
    {
        private OverlayViewModel _overlay;
        private string _currentScreen;

        public MainViewModel(
            IMainContentViewModel main,
            ILoginViewModel login,
            IOverlayService overlayService)
        {
            Main = main;
            Login = login;

            overlayService.Show
                .Subscribe(UpdateOverlay)
                .DisposeWith(this);

            CloseOverlayCommand = ReactiveCommand<object>.Create()
                .DisposeWith(this);

            CloseOverlayCommand.Subscribe(x => ClearOverlay())
                .DisposeWith(this);

            Disposable.Create(() => CloseOverlayCommand = null)
                .DisposeWith(this);

            Navigator.AddRoute(Screens.LOGIN_LAYOUT, OnGoToLogin);
            Navigator.AddRoute(Screens.MAIN_LAYOUT, OnGoToHome);

            using (var dataContext = new LocalDBContext())
            {
                var token = dataContext.Tokens.FirstOrDefault();
                var device = dataContext.Devices.FirstOrDefault();

                if (token != null && device != null)
                {
                    _currentScreen = Screens.MAIN_LAYOUT;
                    CurrentViewModel = Main;   
                } else
                {
                    _currentScreen = Screens.LOGIN_LAYOUT;
                    CurrentViewModel = Login;
                }
            }


        
        }

        private void ChangeViewModel(IViewModel viewModel)
        {
            CurrentViewModel = viewModel;
        }

        private void OnGoToLogin(object obj)
        {
            ChangeViewModel(Login);
            _currentScreen = Screens.LOGIN_LAYOUT;
            OnPropertyChanged(() => CurrentScreen);
            OnPropertyChanged(() => CurrentViewModel);

        }

        private void OnGoToHome(object obj)
        {
            ChangeViewModel(Main);
            _currentScreen = Screens.MAIN_LAYOUT;
            OnPropertyChanged(() => CurrentScreen);
            OnPropertyChanged(() => CurrentViewModel);
        }

        public IMainContentViewModel Main { get; }

        public ILoginViewModel Login { get; }

        public IViewModel CurrentViewModel { get; set; }
        public string CurrentScreen {
            get => _currentScreen;
            set
            {
                _currentScreen = value;
                OnPropertyChanged(() => CurrentScreen);
            }
        }

        public ReactiveCommand<object> CloseOverlayCommand { get; private set; }

        public bool HasOverlay => _overlay != null;

        public string OverlayHeader => _overlay != null ? _overlay.Header : string.Empty;

        public BaseViewModel Overlay => _overlay?.ViewModel;

        private void ClearOverlay()
        {
            using (_overlay.Lifetime)
            {
                UpdateOverlayImpl(null);
            }
        }

        private void UpdateOverlay(OverlayViewModel overlay)
        {
            using (SuspendNotifications())
            {
                if (_overlay != null) ClearOverlay();

                UpdateOverlayImpl(overlay);
            }
        }

        private void UpdateOverlayImpl(OverlayViewModel overlay)
        {
            _overlay = overlay;

            OnPropertyChanged(() => HasOverlay);
            OnPropertyChanged(() => Overlay);
            OnPropertyChanged(() => OverlayHeader);
        }
    }
}