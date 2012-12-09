using System;
using System.Windows.Data;

using TripPoint.WindowsPhone.Utils;

namespace TripPoint.WindowsPhone.Converters
{
    public class BytesToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, 
            System.Globalization.CultureInfo culture)
        {
            return ImageUtils.CreateBitmapFromBytes((byte[])value);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            // not supported
            return null;
        }
    }
}
