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
    /// Interaction logic for OrderDetailItem.xaml
    /// </summary>
    public partial class OrderDetailItem : UserControl
    {
        public OrderDetailItem()
        {
            InitializeComponent();
        }

        public Guid ItemId
        {
            get { return (Guid)GetValue(ItemIdProperty); }
            set { SetValue(ItemIdProperty, value); }
        }

        public static readonly DependencyProperty ItemIdProperty = DependencyProperty.Register("ItemId", typeof(Guid), typeof(OrderDetailItem));

        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }
        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register("ItemName", typeof(string), typeof(OrderDetailItem));

        public string ItemQuantity
        {
            get { return (string)GetValue(ItemQuantityProperty); }
            set { SetValue(ItemQuantityProperty, value); }
        }
        public static readonly DependencyProperty ItemQuantityProperty = DependencyProperty.Register("ItemQuantity", typeof(string), typeof(OrderDetailItem));

        public double ItemPrice
        {
            get { return (double)GetValue(ItemPriceProperty); }
            set { SetValue(ItemPriceProperty, value); }
        }
        public static readonly DependencyProperty ItemPriceProperty = DependencyProperty.Register("ItemPrice", typeof(double), typeof(OrderDetailItem));

        public ImageSource ItemImage
        {
            get { return (ImageSource)GetValue(ItemImageProperty); }
            set { SetValue(ItemImageProperty, value); }
        }
        public static readonly DependencyProperty ItemImageProperty = DependencyProperty.Register("ItemImage", typeof(ImageSource), typeof(OrderDetailItem));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(OrderDetailItem));
    }
}
