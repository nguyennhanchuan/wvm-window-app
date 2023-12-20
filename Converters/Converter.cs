using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VendingMachine.Converters
{
    // IsLessThanConverter //
    public class IsLessThanConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new IsLessThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue = System.Convert.ToDouble(value);
            double compareToValue = System.Convert.ToDouble(parameter);

            return doubleValue < compareToValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // IsGreaterThanConverter //
    public class IsGreaterThanConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new IsGreaterThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue = System.Convert.ToDouble(value);
            double compareToValue = System.Convert.ToDouble(parameter);

            return doubleValue > compareToValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsNullConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new IsNullConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("IsNullConverter can only be used OneWay.");
        }
    }


    public class BooleanToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        public static readonly IValueConverter Instance = new BooleanToVisibilityConverter();
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }

        #endregion
    }

    public class InvertBooleanToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        public static readonly IValueConverter Instance = new InvertBooleanToVisibilityConverter();
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Hidden : Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Hidden;
        }

        #endregion
    }

    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return false;

                var doubleValue = (double)value;
                CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
                return doubleValue.ToString("#,###đ", cul.NumberFormat);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
