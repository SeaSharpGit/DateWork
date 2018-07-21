using System;
using System.Globalization;
using System.Windows.Data;

namespace DateWork.Converters
{
    /// <summary>
    /// 返回value类型；单向
    /// </summary>
    public class ObjectToTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? typeof(object) : value.GetType();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}