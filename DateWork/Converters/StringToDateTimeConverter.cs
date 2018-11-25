using System;
using System.Windows;
using System.Windows.Data;

namespace DateWork.Converters
{
    public class StringToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value.ToString();
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return System.Convert.ToDateTime(str);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var format=parameter==null?"yyyy-MM-dd HH:mm:ss":parameter.ToString();
            if (value is DateTime time)
            {
                return time.ToString(format);
            }
            return "";
        }
    }
}