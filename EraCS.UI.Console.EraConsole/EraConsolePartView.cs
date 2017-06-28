using System;
using System.IO;
using Xamarin.Forms;

namespace EraCS.UI.EraConsole
{
    public abstract class ConsoleLinePart : View
    {
    }

    public class ConsoleStringPart : ConsoleLinePart
    {
        public ConsoleStringPart(Color textColor, string text)
        {
            TextColor = textColor;
            Text = text;
        }

        public virtual Color TextColor { get; }
        public string Text { get; }
    }

    public class ConsoleButtonPart : ConsoleStringPart
    {
        public ConsoleButtonPart(
            string buttonText, 
            string buttonValue, 
            Color textColor, 
            Color highlightColor,
            Action<string> clickAction) 
            : base(textColor, buttonText)
        {
            ButtonValue = buttonValue;
            HighlightColor = highlightColor;
            ClickAction = () =>
            {
                if (Clickable) clickAction(buttonValue);
            };
        }

        public override Color TextColor => Clickable && CursorOn ? HighlightColor : base.TextColor;

        public Color HighlightColor { get; }
        public Action ClickAction { get; }
        public string ButtonValue { get; }

        public bool CursorOn { get; set; }

        public bool Clickable { get; set; } = true;
    }

    public class ConsoleImagePart : ConsoleLinePart
    {
        public ConsoleImagePart(Stream source)
        {
            Source = source;
        }

        public Stream Source { get; }
    }
}