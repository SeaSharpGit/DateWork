using System;
using System.Windows.Data;

namespace DateWork.Converters
{
    public class IsMonthDayToChineseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool flag)
            {
                return flag ? "阴历" : "阳历";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString() == "阴历";
        }
    }
}