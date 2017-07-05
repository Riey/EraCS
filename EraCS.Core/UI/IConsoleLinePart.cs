using SkiaSharp;

namespace EraCS.UI
{
    public interface IConsoleLinePart
    {
        float Width { get; }

        void DrawTo(SKCanvas canvas, float x, float y);
    }
}