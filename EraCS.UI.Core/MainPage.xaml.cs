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
        public MainPage(EraConsole console)
        {
            BindingContext = console;
            InitializeComponent();

            ConsoleGrid.Children[0] = console.View;
        }
    }
}