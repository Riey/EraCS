using System;

using Xamarin.Forms;
using SkiaSharp.Views.Forms;

using EraCS.UI.EraConsole;

namespace EraCS.UI.FrontEnd
{
    public class ConsoleView : SKGLView
    {
        public ConsoleView()
        {
            EnableTouchEvents = true;
        }
        
        private EraConsole.EraConsole _console;

        public EraConsole.EraConsole Console
        {
            get => _console;
            set
            {
                UnRegisterConsole(_console);
                RegisterConsole(value);

                _console = value;
            }
        }

        private void ConsolePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(sender is EraConsole.EraConsole console)
            {
                switch(e.PropertyName)
                {
                    case "Height":
                        HeightRequest = console.Height;
                        break;
                }
            }
        }

        private void RegisterConsole(EraConsole.EraConsole console)
        {
            if(console == null) return;

            console.DrawRequested += InvalidateSurface;
            console.PropertyChanged += ConsolePropertyChanged;
        }

        private void UnRegisterConsole(EraConsole.EraConsole console)
        {
            if(console == null) return;

            console.DrawRequested -= InvalidateSurface;
            console.PropertyChanged -= ConsolePropertyChanged;
        }

        protected override void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
        {
            Console?.Draw(e.Surface.Canvas);
        }

        protected override void OnTouch(SKTouchEventArgs e)
        {
            //TODO
        }
    }
}
