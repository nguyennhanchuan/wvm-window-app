using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VendingMachine.Resources.Control
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : UserControl
    {
        public Product()
        {
            InitializeComponent();
        }

        public string ProductName
        {
            get { return (string)GetValue(ProductNameProperty); }
            set { SetValue(ProductNameProperty, value); }
        }
        public static readonly DependencyProperty ProductNameProperty = DependencyProperty.Register("ProductName", typeof(string), typeof(Product));

        public string ProductId
        {
            get { return (string)GetValue(ProductIdProperty); }
            set { SetValue(ProductIdProperty, value); }
        }
        public static readonly DependencyProperty ProductIdProperty = DependencyProperty.Register("ProductId", typeof(string), typeof(Product));

        public string ProductQuantity
        {
            get { return (string)GetValue(ProductQuantityProperty); }
            set { SetValue(ProductQuantityProperty, value); }
        }
        public static readonly DependencyProperty ProductQuantityProperty = DependencyProperty.Register("ProductQuantity", typeof(string), typeof(Product));

        public string ProductCol
        {
            get { return (string)GetValue(ProductColProperty); }
            set { SetValue(ProductColProperty, value); }
        }
        public static readonly DependencyProperty ProductColProperty = DependencyProperty.Register("ProductCol", typeof(string), typeof(Product));

        public string ProductRow
        {
            get { return (string)GetValue(ProductRowProperty); }
            set { SetValue(ProductRowProperty, value); }
        }
        public static readonly DependencyProperty ProductRowProperty = DependencyProperty.Register("ProductRow", typeof(string), typeof(Product));

        public double ProductPrice
        {
            get { return (double)GetValue(ProductPriceProperty); }
            set { SetValue(ProductPriceProperty, value); }
        }
        public static readonly DependencyProperty ProductPriceProperty = DependencyProperty.Register("ProductPrice", typeof(double), typeof(Product));

        public string ProductDiscount
        {
            get { return (string)GetValue(ProductDiscountProperty); }
            set { SetValue(ProductDiscountProperty, value); }
        }
        public static readonly DependencyProperty ProductDiscountProperty = DependencyProperty.Register("ProductDiscount", typeof(string), typeof(Product));

        public ImageSource ProductImage
        {
            get { return (ImageSource)GetValue(ProductImageProperty); }
            set { SetValue(ProductImageProperty, value); }
        }
        public static readonly DependencyProperty ProductImageProperty = DependencyProperty.Register("ProductImage", typeof(ImageSource), typeof(Product));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(Product));


    }
}
