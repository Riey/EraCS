using SkiaSharp;
using System.Collections.Generic;
using System.Linq;

namespace EraCS.UI.EraConsole
{
    public class ConsoleLine : IConsoleLine
    {
        private float _prevX = 0;

        protected readonly LineAlignment alignment;
        public List<IConsoleLinePart> Parts { get; }

        public ConsoleLine(LineAlignment alignment, float lineHeight)
        {
            this.alignment = alignment;
            Height = lineHeight;
            Parts = new List<IConsoleLinePart>(10);
        }

        public IConsoleLinePart GetPart(float x)
        {
            if (x < _prevX)
                return null;

            foreach (var part in Parts)
            {
                x -= part.Width;
                
                if(x > _prevX) continue;

                return part;
            }

            return null;
        }

        public virtual void DrawTo(SKCanvas canvas, float y)
        {
            float width = Parts.Sum(p => p.Width);

            float x = 0;

            if (alignment == LineAlignment.Center)
                x = (canvas.LocalClipBounds.Width - width) / 2;
            else if (alignment == LineAlignment.Right)
                x = canvas.LocalClipBounds.Width - width;

            _prevX = x;

            foreach (var part in Parts)
            {
                part.DrawTo(canvas, x , y);
                x += part.Width;
            }
        }

        public float Height { get; }
    }
}
