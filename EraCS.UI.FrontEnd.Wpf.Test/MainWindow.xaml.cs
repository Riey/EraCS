using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

            program.Console.Typeface = SKTypeface.FromFile("NanumBarunGothic.otf");
            program.Start();
        }
    }
}
