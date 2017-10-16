using SkiaSharp;

namespace EraCS.UI.EraConsole
{
    public interface IConsoleLine
    {
        IConsoleLinePart GetPart(float x);
        void DrawTo(SKCanvas canvas, float y);
        float Height { get; }
    }
}
