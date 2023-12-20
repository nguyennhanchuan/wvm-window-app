using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VendingMachine.Resources.Control
{
    /// <summary>
    /// Interaction logic for ProductType.xaml
    /// </summary>
    public partial class ProductType : UserControl
    {
        public ProductType()
        {
            InitializeComponent();
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ProductType));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ProductType));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ProductType));

        public double ZoomFactorX
        {
            get { return (double)GetValue(ZoomFactorXProperty); }
            set { SetValue(ZoomFactorXProperty, value); }
        }
        public static readonly DependencyProperty ZoomFactorXProperty = DependencyProperty.Register("ZoomFactorX", typeof(double), typeof(ProductType), new PropertyMetadata(1d));

        public double ZoomFactorY
        {
            get { return (double)GetValue(ZoomFactorYProperty); }
            set { SetValue(ZoomFactorYProperty, value); }
        }
        public static readonly DependencyProperty ZoomFactorYProperty = DependencyProperty.Register("ZoomFactorY", typeof(double), typeof(ProductType), new PropertyMetadata(1d));

        public double XTranslate
        {
            get { return (double)GetValue(XTranslateProperty); }
            set { SetValue(XTranslateProperty, value); }
        }
        public static readonly DependencyProperty XTranslateProperty = DependencyProperty.Register("XTranslate", typeof(double), typeof(ProductType), new PropertyMetadata(0d));    
        
        public double YTranslate
        {
            get { return (double)GetValue(YTranslateProperty); }
            set { SetValue(YTranslateProperty, value); }
        }
        public static readonly DependencyProperty YTranslateProperty = DependencyProperty.Register("YTranslate", typeof(double), typeof(ProductType), new PropertyMetadata(0d));

        public double BorderWidth
        {
            get { return (double)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }
        public static readonly DependencyProperty BorderWidthProperty = DependencyProperty.Register("BorderWidth", typeof(double), typeof(ProductType), new PropertyMetadata(0d));
    }
}
