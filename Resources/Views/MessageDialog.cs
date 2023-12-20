using MahApps.Metro.Controls.Dialogs;
using System.Windows.Markup;
using VendingMachine.Models;
using VendingMachine.ViewModels;

namespace VendingMachine.Resources.Views
{
    [ContentProperty("DialogBody")]
    public sealed class MessageDialog : BaseMetroDialog
    {
        private readonly Message _message;

        public MessageDialog(Message message)
        {
            _message = message;

            Title = _message.Header;
            Content = _message.ViewModel;
        }

        public ICloseableViewModel CloseableContent => _message.ViewModel;
    }
}