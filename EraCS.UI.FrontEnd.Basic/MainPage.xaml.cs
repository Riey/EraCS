using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EraCS.UI.FrontEnd.Basic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly IEraConsole _console;

        public MainPage(IEraConsole console)
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