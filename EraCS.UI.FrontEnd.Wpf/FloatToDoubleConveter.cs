using System;
using System.Globalization;
using System.Windows.Data;

namespace EraCS.UI.FrontEnd.Wpf
{
    public class FloatToDoubleConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float f) return (double)f;

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}