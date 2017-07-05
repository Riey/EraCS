using System;
using System.Globalization;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EraCS.UI.FrontEnd.Basic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly IEraConsole _console;

        public MainPage(IEraConsole console)
        {
            _console = console;
            BindingContext = console;

            Resources = new ResourceDictionary();

            InitializeComponent();
            console.DrawRequested += ConsoleView.InvalidateSurface;
            
        }

        private void ConsoleEditor_OnCompleted(object sender, EventArgs e)
        {
            _console.OnTextEntered(ConsoleEditor.Text);
            ConsoleEditor.Text = string.Empty;
        }

        private void ConsoleView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            _console.Draw(e.Surface.Canvas);
        }
    }

    public class DoubleConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i) return (double) i;

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
            if(value is SKColor c) return c.ToFormsColor();

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}