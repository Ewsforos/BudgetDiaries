using System;
using Windows.UI.Xaml.Data;

namespace AVL.Converters
{
    public class NegotiveDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return -1 * (double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return -1 * (double)value;
        }
    }
}
