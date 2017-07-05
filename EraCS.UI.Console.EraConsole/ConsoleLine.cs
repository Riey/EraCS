using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace EraCS.UI.EraConsole
{
    public class ConsoleLine : IConsoleLine
    {
        public List<IConsoleLinePart> Parts { get; }

        public ConsoleLine(float lineHeight)
        {
            Height = lineHeight;
            Parts = new List<IConsoleLinePart>(10);
        }

        public IConsoleLinePart GetPart(float x)
        {
            foreach (var part in Parts)
            {
                x -= part.Width;

                if(x > 0) continue;

                return part;
            }

            return null;
        }

        public virtual void DrawTo(SKCanvas canvas, float y)
        {
            float x = 0;

            foreach (var part in Parts)
            {
                part.DrawTo(canvas, x , y);
                x += part.Width;
            }
        }

        public float Height { get; }
    }
}
