using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VendingMachine.Services;

namespace VendingMachine.Resources.Views
{
    /// <summary>
    /// Interaction logic for MainWindowV2.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDisposable _disposable;
        public MainWindow(IMessageService messageService, ISchedulerService schedulerService)
        {
           InitializeComponent();
        }


        private void HandleClosed(object sender, EventArgs e)
        {
            _disposable.Dispose();
        }

        //private IObservable<Unit> ShowDialogAsync(MessageDialog dialog)
        //{
        //    var settings = new MetroDialogSettings
        //    {
        //        AnimateShow = true,
        //        AnimateHide = true,
        //        ColorScheme = MetroDialogColorScheme.Accented
        //    };

        //    return this.ShowMetroDialogAsync(dialog, settings)
        //        .ToObservable()
        //        .SelectMany(x => dialog.CloseableContent.Closed, (x, y) => x)
        //        .SelectMany(x => this.HideMetroDialogAsync(dialog)
        //            .ToObservable(), (x, y) => x)
        //        .Take(1);
        //}

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        public string IsEmpty()
        {
            return "";
        }

        //public void Canvas_Drop(object sender, DragEventArgs e)
        //{
        //}

        //public void Canvas_DragOver(object sender, DragEventArgs e)
        //{
        //    Point dropPosition = e.GetPosition(CanvasDrag);

        //    Canvas.SetLeft(PaymentDrag, dropPosition.X);
        //    Canvas.SetRight(PaymentDrag, dropPosition.Y);
        //}
    }
}
