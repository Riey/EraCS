using SkiaSharp;

namespace EraCS.UI
{
    public interface IConsoleLinePart
    {
        float Width { get; }

        void DrawTo(SKCanvas canvas, float x, float y);

        void OnCursorOver(float x, float y);
        void OnCursorEntered();
        void OnCursorExited();
        void OnClicked(float x, float y);
    }
}