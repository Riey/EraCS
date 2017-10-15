using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EraCS.UI.FrontEnd.Wpf
{
    public class LineHeightToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float f) return new Thickness(0, f / 2, 0, 0);

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}