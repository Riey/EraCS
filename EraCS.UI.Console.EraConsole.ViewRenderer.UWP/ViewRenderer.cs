using EraCS.UI.EraConsole;
using EraCS.UI.EraConsole.ViewRenderer.UWP;
using System;
using System.IO;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ConsoleStringPart), typeof(ConsoleStringRenderer))]
[assembly: ExportRenderer(typeof(ConsoleButtonPart), typeof(ConsoleButtonRenderer))]
[assembly: ExportRenderer(typeof(ConsoleImagePart), typeof(ConsoleImageRenderer))]

namespace EraCS.UI.EraConsole.ViewRenderer.UWP
{
    static class ColorExtensions
    {
        public static Windows.UI.Color ToUwpColor(this Xamarin.Forms.Color c) =>
            Windows.UI.Color.FromArgb(
                (byte) (c.A * 255),
                (byte) (c.R * 255),
                (byte) (c.G * 255),
                (byte) (c.B * 255));
    }

    public class ConsoleStringRenderer : ViewRenderer<ConsoleStringPart, TextBlock>
    {
        internal static TextBlock MakeTextBlock(ConsoleStringPart part) =>
            new TextBlock
            {
                Foreground = new SolidColorBrush(part.TextColor.ToUwpColor()),
                Text = part.Text
            };

        protected override void OnElementChanged(ElementChangedEventArgs<ConsoleStringPart> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                SetNativeControl(MakeTextBlock(e.NewElement));
            }
        }
    }

    public class ConsoleButtonRenderer : ViewRenderer<ConsoleButtonPart, TextBlock>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ConsoleButtonPart> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Control.PointerEntered -= OnPointerEntered;
                Control.PointerExited -= OnPointerExited;

                Control.Tapped -= OnClicked;

            }

            if (e.NewElement != null)
            {
                SetNativeControl(ConsoleStringRenderer.MakeTextBlock(e.NewElement));

                Control.Foreground = new SolidColorBrush(Element.TextColor.ToUwpColor());

                Control.PointerEntered += OnPointerEntered;
                Control.PointerExited += OnPointerExited;

                Control.Tapped += OnClicked;

                Control.Foreground = new SolidColorBrush(e.NewElement.TextColor.ToUwpColor());
            }
        }

        private void OnClicked(object sender, TappedRoutedEventArgs e)
        {
            if (Control == null || Element == null) return;

            e.Handled = true;

            Element.ClickAction();
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (Control == null || Element == null) return;

            e.Handled = true;

            Element.CursorOn = false;
            Control.Foreground = new SolidColorBrush(Element.TextColor.ToUwpColor());
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (Control == null || Element == null) return;

            e.Handled = true;

            Element.CursorOn = true;
            Control.Foreground = new SolidColorBrush(Element.TextColor.ToUwpColor());
        }
    }

    public class ConsoleImageRenderer : ViewRenderer<ConsoleImagePart, Windows.UI.Xaml.Controls.Image>
    {
        protected override async void OnElementChanged(ElementChangedEventArgs<ConsoleImagePart> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                SetNativeControl(new Image());

                var image = new BitmapImage();

                await image.SetSourceAsync(e.NewElement.Source.AsRandomAccessStream());
                e.NewElement.Source.Dispose();

                Control.Source = image;
            }
        }
    }
}
