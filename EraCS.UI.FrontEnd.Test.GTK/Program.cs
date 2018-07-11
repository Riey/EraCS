using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

/*
 * This wasn't work because SkiaSharp doesn't offer Gtk support yet
 */

namespace EraCS.UI.FrontEnd.Test.GTK
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Gtk.Application.Init();
            Forms.Init();

            GLib.ExceptionManager.UnhandledException += e =>
            {
                System.Diagnostics.Debug.WriteLine(e.ExceptionObject.ToString());
            };

            var app = new TestApp();
            var window = new FormsWindow();

            window.LoadApplication(app);
            window.Title = "Test";
            window.ShowAll();
            
            Gtk.Application.Run();
        }
    }
}
