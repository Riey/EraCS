using Xamarin.Forms;

namespace EraCS
{
    public delegate void TextEnteredHandler(string text);

    public interface IEraConsole
    {
        Color ConsoleTextColor { get; set; }
        Color ConsoleBackColor { get; set; }
        Color ConsoleHighlightColor { get; set; }
        event TextEnteredHandler TextEntered;
        void OnTextEntered(string text);

        void DeActiveButtons();

        View View { get; }
    }
}
