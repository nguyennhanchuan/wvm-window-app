using VendingMachine.Commands;

namespace VendingMachine.ViewModels
{
    public interface ILoginViewModel : IViewModel
    {
        IDiagnosticsViewModel Diagnostics { get; }
        ReactiveCommand<object> CloseOverlayCommand { get; }
        bool HasOverlay { get; }
        string OverlayHeader { get; }

        bool ShowPassword { get; set; }
        BaseViewModel Overlay { get; }
    }
}