using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using Xamarin.Forms;

namespace EraCS.Console
{
    public abstract class ConsoleLinePart
    {

    }

    public class ConsoleStringPart : ConsoleLinePart
    {
        public ConsoleStringPart(Color textColor, string text)
        {
            TextColor = textColor;
            Text = text;
        }

        public Color TextColor { get; }
        public string Text { get; }
    }

    public class ConsoleButtonPart : ConsoleLinePart
    {
        public ConsoleButtonPart(ConsoleLinePart part, string buttonValue, ICommand clickCommand)
        {
            Part = part;
            ButtonValue = buttonValue;
            ClickCommand = clickCommand;
        }

        public ConsoleLinePart Part { get; }
        public string ButtonValue { get; }
        public ICommand ClickCommand { get; }
    }

    public class ConsoleImagePart : ConsoleLinePart
    {
        public ConsoleImagePart(ImageSource source)
        {
            Source = source;
        }

        public ImageSource Source { get; }
    }

    static class ConsoleViewFactory
    {
        public static View MakeView(ConsoleLinePart part)
        {
            switch(part)
            {
                case ConsoleStringPart strPart: return MakeLabel(strPart.TextColor, strPart.Text);
                case ConsoleImagePart imgPart: return MakeImage(imgPart.Source);
                case ConsoleButtonPart btnPart: return MakeButton(MakeView(btnPart.Part), btnPart.ButtonValue, btnPart.ClickCommand);
            }

            throw new ArgumentException();
        }

        private static Label MakeLabel(Color textColor, string text)
        {
            var label = new Label() { TextColor = textColor, Text = text };
            label.SetBinding(View.BackgroundColorProperty, new Binding("ConsoleBackColor", BindingMode.OneWay));

            return label;
        }

        private static Image MakeImage(ImageSource source) => new Image() { Source = source };

        private static T MakeButton<T>(T part, string btnValue, ICommand command) where T:View
        {
            part.GestureRecognizers.Add(new TapGestureRecognizer() { Command = command, CommandParameter = btnValue });
            return part;
        }
    }
}
