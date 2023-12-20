namespace VendingMachine.ViewModels
{
    public interface IModifyResourceViewModel : ICloseableViewModel
    {
        string Path { get; }

        string Json { get; set; }
    }
}