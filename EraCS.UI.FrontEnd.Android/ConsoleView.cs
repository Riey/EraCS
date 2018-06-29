using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using SkiaSharp.Views.Android;

namespace EraCS.UI.FrontEnd.Android
{
    public class ConsoleView : SKCanvasView
    {
        private EraConsole.EraConsole _console;
        public EraConsole.EraConsole Console
        {
            get => _console;
            set
            {
                if(_console != null)
                {
                    _console.DrawRequested -= Invalidate;

                }

                _console = value ?? throw new ArgumentNullException(nameof(Console));

                value.DrawRequested += Invalidate;
            }
        }

        private void Init()
        {
            PaintSurface += this.ConsoleView_PaintSurface;
        }

        private void ConsoleView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            _console?.Draw(e.Surface.Canvas);
        }

        public ConsoleView(Context context) : base(context)
        {
            Init();
        }

        public ConsoleView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        private DateTime _preTouchTime = DateTime.Now;
        private readonly TimeSpan ClickInterval = new TimeSpan(0, 0, 0, 0, 150);

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (_console == null) return false;

            float x = e.GetX(), y = e.GetY();
            
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _console.OnCursorMoved(x, y);
                    _preTouchTime = DateTime.Now;
                    return true;

                case MotionEventActions.Move:
                    _console.OnCursorMoved(x, y);
                    return true;

                case MotionEventActions.Up:
                    var curTouchTime = DateTime.Now;
                    _console.OnCursorMoved(x, y);
                    if (curTouchTime - _preTouchTime < ClickInterval)
                    {
                        //Clicked
                        _console.OnClicked(x, y);
                    }
                    return true;
            }
            return base.OnTouchEvent(e);
        }
    }
}
