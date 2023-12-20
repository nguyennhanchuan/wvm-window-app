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
    /// Interaction logic for CartTab.xaml
    /// </summary>
    public partial class CartTab : UserControl
    {
        public CartTab()
        {
            InitializeComponent();
        }

        public string CanvasLeft
        {
            get { return (string)GetValue(CanvasLeftProperty); }
            set { SetValue(CanvasLeftProperty, value); }
        }
        public static readonly DependencyProperty CanvasLeftProperty = DependencyProperty.Register("CanvasLeft", typeof(string), typeof(CartTab));

        public string CanvasTop
        {
            get { return (string)GetValue(CanvasTopProperty); }
            set { SetValue(CanvasTopProperty, value); }
        }
        public static readonly DependencyProperty CanvasTopProperty = DependencyProperty.Register("CanvasTop", typeof(string), typeof(CartTab));

        public string CanvasRight
        {
            get { return (string)GetValue(CanvasRightProperty); }
            set { SetValue(CanvasRightProperty, value); }
        }
        public static readonly DependencyProperty CanvasRightProperty = DependencyProperty.Register("CanvasRight", typeof(string), typeof(CartTab));

        public string CanvasBottom
        {
            get { return (string)GetValue(CanvasBottomProperty); }
            set { SetValue(CanvasBottomProperty, value); }
        }
        public static readonly DependencyProperty CanvasBottomProperty = DependencyProperty.Register("CanvasBottom", typeof(string), typeof(CartTab));
    }
}
