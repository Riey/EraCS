using System;
using SkiaSharp;

namespace EraCS.UI.EraConsole
{
    public class ConsoleStringPart : IConsoleLinePart
    {
        public ConsoleStringPart(SKColor textColor, string text, float textSize, SKTypeface typeface)
        {
            TextColor = textColor;
            Text = text;
            Paint = new SKPaint
            {
                Color = textColor,
                HintingLevel = SKPaintHinting.Full,
                IsAntialias = true,
                IsDither = true,
                IsAutohinted = true,
                SubpixelText = true,
                TextSize = textSize,
                Typeface = typeface
            };
        }

        protected SKPaint Paint { get; }
        public SKColor TextColor { get; }
        public string Text { get; }

        protected virtual SKColor GetTextColor() => TextColor;

        public void DrawTo(SKCanvas canvas, float x, float y)
        {
            Paint.Color = GetTextColor();
            canvas.DrawText(Text, x, y, Paint);
        }

        public float Width => Paint.MeasureText(Text);
    }

    public class ConsoleButtonPart : ConsoleStringPart
    {
        public ConsoleButtonPart(
            string buttonText,
            float textSize,
            SKTypeface typeface,
            string buttonValue,
            SKColor textColor,
            SKColor highlightColor,
            Action<string> clickAction) 
            : base(textColor, buttonText, textSize, typeface)
        {
            ButtonValue = buttonValue;
            HighlightColor = highlightColor;
            ClickAction = () =>
            {
                if (Clickable) clickAction(buttonValue);
            };
        }

        protected override SKColor GetTextColor() =>
            CursorOn && Clickable ? HighlightColor : TextColor;

        public SKColor HighlightColor { get; }
        public Action ClickAction { get; }
        public string ButtonValue { get; }

        public bool CursorOn { get; set; }

        public bool Clickable { get; set; } = true;
    }

    public class ConsoleImagePart : IConsoleLinePart
    {
        public ConsoleImagePart(SKImage image)
        {
            Image = image;
        }

        public SKImage Image { get; }

        public void DrawTo(SKCanvas canvas, float x, float y)
        {
            canvas.DrawImage(Image, x, y);
        }

        public float Width => Image.Width;
    }
}