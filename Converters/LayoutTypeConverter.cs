using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VendingMachine.Navigation;

namespace VendingMachine.Converters
{
    public sealed class LayoutTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string) value)
            {
                case Screens.MAIN_LAYOUT:
                    return Application.Current.FindResource("Desktop_Portrait_Layout");
                case Screens.LOGIN_LAYOUT:
                    return Application.Current.FindResource("Login_layout");
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}