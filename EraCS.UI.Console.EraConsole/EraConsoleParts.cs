using System;
using SkiaSharp;

namespace EraCS.UI.EraConsole
{
    public class ConsoleStringPart : IConsoleLinePart
    {
        protected readonly float textHeight;

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

            textHeight = Paint.FontMetrics.Ascent;
            Width = Paint.MeasureText(text);
        }

        protected SKPaint Paint { get; }
        public SKColor TextColor { get; }
        public string Text { get; }

        protected virtual SKColor GetTextColor() => TextColor;

        public void DrawTo(SKCanvas canvas, float x, float y)
        {
            Paint.Color = GetTextColor();
            canvas.DrawText(Text, x, y - textHeight, Paint);
        }

        public virtual void OnCursorOver(float x, float y)
        {
        }

        public virtual void OnCursorEntered()
        {
        }

        public virtual void OnCursorExited()
        {
        }

        public virtual void OnClicked(float x, float y)
        {
        }

        public float Width { get; }
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

        public override void OnClicked(float x, float y)
        {
            base.OnClicked(x, y);
            ClickAction();
        }

        public override void OnCursorEntered()
        {
            base.OnCursorEntered();
            CursorOn = true;
        }

        public override void OnCursorExited()
        {
            base.OnCursorExited();
            CursorOn = false;
        }
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

        public virtual void OnCursorOver(float x, float y)
        {
        }

        public virtual void OnCursorEntered()
        {
        }

        public virtual void OnCursorExited()
        {
        }

        public virtual void OnClicked(float x, float y)
        {
        }

        public float Width => Image.Width;
    }
}