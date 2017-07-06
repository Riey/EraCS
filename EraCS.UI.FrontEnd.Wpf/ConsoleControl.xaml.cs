using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;

namespace EraCS.UI.FrontEnd.Wpf
{
    /// <summary>
    /// ConsoleControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ConsoleControl : UserControl
    {
        public IEraConsole Console
        {
            get => (IEraConsole) GetValue(ConsoleProperty);
            set => SetValue(ConsoleProperty, value);
        }

        private static void OnConsoleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ConsoleControl control && e.NewValue is IEraConsole console)
            {
                if (e.OldValue is IEraConsole oldConsole)
                {
                    if (console == oldConsole) return;

                    oldConsole.DrawRequested -= control.ConsoleView.InvalidateVisual;
                }
                console.DrawRequested += control.ConsoleView.InvalidateVisual;
                control.DataContext = console;
            }
        }

        public static readonly DependencyProperty ConsoleProperty =
            DependencyProperty.Register(
                nameof(Console), typeof(IEraConsole), typeof(ConsoleControl),
                new FrameworkPropertyMetadata(OnConsoleChanged)
                );

        public ConsoleControl()
        {
            InitializeComponent();
        }

        private void ConsoleView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            Console?.Draw(e.Surface.Canvas);
        }

        private void ConsoleView_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(ConsoleView);
            Console?.OnClicked((float)p.X, (float)p.Y);
        }

        private void ConsoleView_OnMouseMove(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(ConsoleView);
            Console?.OnCursorMoved((float)p.X, (float)p.Y);
        }
    }

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
