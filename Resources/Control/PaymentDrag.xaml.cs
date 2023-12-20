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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VendingMachine.Resources.Control
{
    /// <summary>
    /// Interaction logic for PaymentDrag.xaml
    /// </summary>
    public partial class PaymentDrag : UserControl
    {
        public PaymentDrag()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(PaymentDrag));

        public string CanvasLeft
        {
            get { return (string)GetValue(CanvasLeftProperty); }
            set { SetValue(CanvasLeftProperty, value); }
        }
        public static readonly DependencyProperty CanvasLeftProperty = DependencyProperty.Register("CanvasLeft", typeof(string), typeof(PaymentDrag));

        public string CanvasTop
        {
            get { return (string)GetValue(CanvasTopProperty); }
            set { SetValue(CanvasTopProperty, value); }
        }
        public static readonly DependencyProperty CanvasTopProperty = DependencyProperty.Register("CanvasTop", typeof(string), typeof(PaymentDrag));

        public string CanvasRight
        {
            get { return (string)GetValue(CanvasRightProperty); }
            set { SetValue(CanvasRightProperty, value); }
        }
        public static readonly DependencyProperty CanvasRightProperty = DependencyProperty.Register("CanvasRight", typeof(string), typeof(PaymentDrag));

        public string CanvasBottom
        {
            get { return (string)GetValue(CanvasBottomProperty); }
            set { SetValue(CanvasBottomProperty, value); }
        }
        public static readonly DependencyProperty CanvasBottomProperty = DependencyProperty.Register("CanvasBottom", typeof(string), typeof(PaymentDrag));

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
            }
        }
    }
}
