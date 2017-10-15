using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SkiaSharp;
using SkiaSharp.Views.WPF;

namespace EraCS.UI.FrontEnd.Wpf
{
    public class SkColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SKColor c) return new SolidColorBrush(c.ToColor());

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}