namespace VendingMachine.ViewModels
{
    public interface IDiagnosticsViewModel : IViewModel
    {
        string Cpu { get; }
        string ManagedMemory { get; }
        string TotalMemory { get; }
    }
}