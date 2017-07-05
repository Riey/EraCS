using System;
using SkiaSharp;
using System.ComponentModel;

namespace EraCS
{
    public delegate void TextEnteredHandler(string text);

    public interface IEraConsole : INotifyPropertyChanged
    {
        SKColor ConsoleTextColor { get; set; }
        SKColor ConsoleBackColor { get; set; }
        SKColor ConsoleHighlightColor { get; set; }
        float Height { get; }

        event TextEnteredHandler TextEntered;
        event Action DrawRequested;

        void OnCursorMoved(float x, float y);
        void OnClicked(float x, float y);
        void OnTextEntered(string text);

        void DeActiveButtons();

        void Draw(SKCanvas c);
    }
}
