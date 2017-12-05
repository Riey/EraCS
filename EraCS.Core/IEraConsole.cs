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

        bool NeedRedraw { get; }

        event TextEnteredHandler TextEntered;
        event Action Clicked;
        event Action DrawRequested;


        /// <summary>
        /// Must be run on ui thread
        /// </summary>
        void OnCursorMoved(float x, float y);
        /// <summary>
        /// Must be run on ui thread
        /// </summary>
        void OnClicked(float x, float y);
        /// <summary>
        /// Must be run on ui thread
        /// </summary>
        void OnTextEntered(string text);
        /// <summary>
        /// Must be run on ui thread
        /// </summary>
        void OnDrawRequested();

        void DeActiveButtons();
        
        void Draw(SKCanvas c);
    }
}
