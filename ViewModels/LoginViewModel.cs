using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls;
using VendingMachine.Commands;
using VendingMachine.DataFile;
using VendingMachine.Extensions;
using VendingMachine.Models;
using VendingMachine.Navigation;
using VendingMachine.Services;

namespace VendingMachine.ViewModels
{
    public sealed class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        private OverlayViewModel _overlay;
        private readonly IApiService _apiService;
        private readonly IUartService _uartService;

        private readonly ISchedulerService _schedulerService;

        private bool _showPassword = false;
        private string _username;
        private string _password;
        private string _deviceCode;

        public LoginViewModel(
            IOverlayService overlayService,
            IDiagnosticsViewModel diagnosticsViewModel,
            IApiService apiService,
            ISchedulerService schedulerService)
        {
            Diagnostics = diagnosticsViewModel;

            _apiService = apiService;
            _schedulerService = schedulerService;

            overlayService.Show
                .Subscribe(UpdateOverlay)
                .DisposeWith(this);

            CloseOverlayCommand = ReactiveCommand<object>.Create()
                .DisposeWith(this);

            CloseOverlayCommand.Subscribe(x => ClearOverlay())
                .DisposeWith(this);


            TogglePasswordCommand = ReactiveCommand.Create()
         .DisposeWith(this);

            TogglePasswordCommand.Subscribe(x => TogglePassword((PasswordBox)x));

            LoginCommand = ReactiveCommand.Create()
             .DisposeWith(this);

            LoginCommand.Subscribe(x => Login((PasswordBox)x));


            PasswordChangedCommand = ReactiveCommand.Create()
             .DisposeWith(this);

            PasswordChangedCommand.Subscribe(x => OnPasswordChanged((PasswordBox)x));


            PasswordTextChangedCommand = ReactiveCommand.Create()
             .DisposeWith(this);

            PasswordTextChangedCommand.Subscribe(x => OnPasswordTextChanged((PasswordBox)x));

            Disposable.Create(() => CloseOverlayCommand = null)
                .DisposeWith(this);

        }

        private void OnPasswordChanged(PasswordBox passwordBox)
        {
            _password = passwordBox.Password;
            OnPropertyChanged(() => Password);
        }

        private void OnPasswordTextChanged(PasswordBox passwordBox)
        {
            if (_showPassword)
            {
                passwordBox.Password = _password;
            }
        }

        private void Login(PasswordBox passwordBox)
        {
            _apiService.Login(_username, _password, _deviceCode).ObserveOn(_schedulerService.Dispatcher)
               .Subscribe((loginResponse) =>
               {
                   var device = new MerchantDevice()
                   {
                       Id = loginResponse.MainMerchantDevice.Id,
                       MerchantId = loginResponse.MainMerchantDevice.MerchantId,
                       Code = loginResponse.MainMerchantDevice.Code,
                       DeviceId = loginResponse.MainMerchantDevice.DeviceId,
                       DeviceName = loginResponse.MainMerchantDevice.DeviceName,
                       MerchantStoreId = loginResponse.MainMerchantDevice.MerchantStoreId,
                       Name = loginResponse.MainMerchantDevice.DeviceName,
                   };
                   var token = new Token()
                   {
                       UserId = loginResponse.SysUser.Id.ToString(),
                       AccessToken = loginResponse.AccessToken,
                       RefreshToken = loginResponse.RefreshToken
                   };

                   using (var dataContext = new LocalDBContext())
                   {
                       dataContext.Tokens.Add(token);
                       dataContext.Devices.Add(device);
                       dataContext.SaveChanges();
                   }
                   Navigator.Navigate(Screens.MAIN_LAYOUT);
               })
               .DisposeWith(this);
        }

        private void TogglePassword(PasswordBox passwordBox)
        {
            if (_showPassword)
            {
                passwordBox.Password = _password;
            }
            else
            {
                _password = passwordBox.Password;
            }
            _showPassword = !_showPassword;
            OnPropertyChanged(() => ShowPassword);
        }

        public IDiagnosticsViewModel Diagnostics { get; }

        public ReactiveCommand<object> CloseOverlayCommand { get; private set; }

        public ReactiveCommand<object> TogglePasswordCommand { get; private set; }

        public ReactiveCommand<object> LoginCommand { get; private set; }

        public ReactiveCommand<object> PasswordChangedCommand { get; private set; }

        public ReactiveCommand<object> PasswordTextChangedCommand { get; private set; }


        public bool HasOverlay => _overlay != null;

        public string OverlayHeader => _overlay != null ? _overlay.Header : string.Empty;

        public bool ShowPassword
        {
            get => _showPassword;
            set
            {
                _showPassword = value;
                OnPropertyChanged(() => ShowPassword);
            }

        }

        public string UserName
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(() => UserName);
            }

        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(() => Password);
            }

        }

        public string DeviceCode
        {
            get => _deviceCode;
            set
            {
                _deviceCode = value;
                OnPropertyChanged(() => DeviceCode);
            }

        }

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