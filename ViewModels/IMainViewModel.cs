using VendingMachine.Commands;

namespace VendingMachine.ViewModels
{
    public interface IMainViewModel : IViewModel
    {
        string CurrentScreen { get; set; }
        IViewModel CurrentViewModel { get; set; }
        IMainContentViewModel Main { get; }
        ILoginViewModel Login { get; }

        ReactiveCommand<object> CloseOverlayCommand { get; }
        bool HasOverlay { get; }
        string OverlayHeader { get; }
        BaseViewModel Overlay { get; }
    }
}