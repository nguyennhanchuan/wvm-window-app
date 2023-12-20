using System;

namespace VendingMachine.ViewModels
{
    public sealed class MessageViewModel : OverlayViewModel<ICloseableViewModel>
    {
        public MessageViewModel(string header, ICloseableViewModel viewModel, IDisposable lifetime)
            : base(header, viewModel, lifetime)
        {
        }
    }
}