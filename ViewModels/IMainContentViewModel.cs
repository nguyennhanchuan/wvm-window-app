using VendingMachine.Commands;

namespace VendingMachine.ViewModels
{
    public interface IMainContentViewModel : IViewModel
    {
        IDiagnosticsViewModel Diagnostics { get; }
    }
}