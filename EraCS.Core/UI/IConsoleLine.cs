using System.Collections.Generic;
using SkiaSharp;

namespace EraCS.UI
{
    public interface IConsoleLine
    {
        IConsoleLinePart GetPart(float x);
        void DrawTo(SKCanvas canvas, float y);
        float Height { get; }
    }
}
