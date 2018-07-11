using System;

using Xamarin.Forms;

using EraCS.Core.Test.Program;

namespace EraCS.UI.FrontEnd.Test
{
    public class TestPage : ContentPage
    {
        private event TextEnteredHandler TextEntered;

        public TestPage()
        {
            var consoleView = new ConsoleView();
            var editor = new Editor();

            var program = new TestProgram();

            TextEntered += program.Console.OnTextEntered;

            consoleView.Console = program.Console;
            editor.Completed += EditorCompleted;
            editor.Text = "Hello";


            Content =
                new StackLayout()
                {
                    Children =
                        {
                            consoleView,
                            editor
                        }
                };

            program.Start();
        }

        private void EditorCompleted(object sender, EventArgs e)
        {
            if(sender is Editor editor)
            {
                TextEntered?.Invoke(editor.Text);
                editor.Text = "";
            }
        }
    }
}
