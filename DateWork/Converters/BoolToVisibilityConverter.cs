using System;
using System.Windows;
using System.Windows.Data;

namespace DateWork.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var flag = System.Convert.ToBoolean(value);
            
            if (parameter == null)
            {
                return flag ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return flag ? Visibility.Collapsed : Visibility.Visible;
            } 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (!(value is Visibility)))
            {
                return false;
            }
            var visibility = (Visibility)value;
            return visibility == Visibility.Visible;
        }
    }
}