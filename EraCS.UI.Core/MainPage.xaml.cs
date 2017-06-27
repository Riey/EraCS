using EraCS.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EraCS.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly EraConsole _console;

        public MainPage(EraConsole console)
        {
            _console = console;
            BindingContext = console;
            InitializeComponent();

            ConsoleGrid.Children[0] = console.View;
        }

        private void ConsoleEditor_OnCompleted(object sender, EventArgs e)
        {
            _console.OnTextEntered(ConsoleEditor.Text);
            ConsoleEditor.Text = string.Empty;
        }
    }
}