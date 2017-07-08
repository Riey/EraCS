using System.Windows;
using System.Windows.Input;
using EraCS.Core.Test.Program;
using SkiaSharp;

namespace EraCS.UI.FrontEnd.Wpf.Test
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var program = new TestProgram();
            ConsoleControl.Console = program.Console;
            DataContext = program.Console;

            program.Console.Typeface = SKTypeface.FromFile("NanumBarunGothic.otf");
            program.Start();
        }

        private void ConsoleTb_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown && e.Key == Key.Enter)
            {
                ConsoleControl.Console.OnTextEntered(ConsoleTb.Text);
                ConsoleTb.Text = "";
            }
        }
    }
}
