using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EraCS.Console
{
    public delegate void TextEnteredHandler(string text);

    public sealed class EraConsole : BindableObject
    {
        private List<ButtonClickCommand> _activeButtons = new List<ButtonClickCommand>(100);
        private readonly StackLayout _stack;
        
        private bool _lastLineIsTemporary = false;
        private bool _blankLineFlag = true;

        private StackLayout LastLine => (StackLayout)_stack.Children[_stack.Children.Count - 1];

        public View View => _stack;

        public static readonly BindableProperty ConsoleTextColorProperty =
            BindableProperty.Create(
                propertyName: nameof(ConsoleTextColor),
                returnType: typeof(Color),
                declaringType: typeof(EraConsole),
                defaultValue: Color.White);

        public static readonly BindableProperty ConsoleBackColorProperty =
            BindableProperty.Create(
                propertyName: nameof(ConsoleBackColor),
                returnType: typeof(Color),
                declaringType: typeof(EraConsole),
                defaultValue: Color.Black);

        public EraConsole()
        {
            _stack = new StackLayout() { VerticalOptions = LayoutOptions.EndAndExpand };
            _stack.SetBinding(StackLayout.BackgroundColorProperty, "ConsoleBackColor", BindingMode.OneWay);
        }

        private void SetValueAndRaise(BindableProperty property, object value, [CallerMemberName] string propertyName = "")
        {
            SetValue(property, value);
            OnPropertyChanged(propertyName);
        }

        public Color ConsoleTextColor { get => (Color)GetValue(ConsoleTextColorProperty); set => SetValueAndRaise(ConsoleTextColorProperty, value); }
        public Color ConsoleBackColor { get => (Color)GetValue(ConsoleBackColorProperty); set => SetValueAndRaise(ConsoleBackColorProperty, value); }
        public bool SkipPrint { get; set; }
        public LayoutOptions Alignment { get; set; } = LayoutOptions.Center;

        private void AddBlankLine()
        {
            var line = new StackLayout() { HorizontalOptions = Alignment, Orientation = StackOrientation.Horizontal };
            line.SetBinding(StackLayout.BackgroundColorProperty, "ConsoleBackColor", BindingMode.OneWay);
            _stack.Children.Add(line);
        }

        private void AddPart(ConsoleLinePart part)
        {
            if (SkipPrint) return;

            if (_lastLineIsTemporary)
            {
                _stack.Children.RemoveAt(_stack.Children.Count - 1);
                _lastLineIsTemporary = false;
            }

            if(_blankLineFlag || _stack.Children.Count == 0)
            {
                AddBlankLine();
                _blankLineFlag = false;
            }

            LastLine.Children.Add(ConsoleViewFactory.MakeView(part));
        }

        public void OnTextEntered(string value)
        {
            TextEntered?.Invoke(value);
        }

        public event TextEnteredHandler TextEntered; 

        public void Print(string str) => AddPart(new ConsoleStringPart(ConsoleTextColor, str));
        public void PrintLine(string str) { Print(str); NewLine(); }
        public void PrintButton(string text, string value)
        {
            var com = new ButtonClickCommand(OnTextEntered);
            _activeButtons.Add(com);
            AddPart(
                new ConsoleButtonPart(
                    new ConsoleStringPart(ConsoleTextColor, text),
                    value, 
                    com));
        }

        public void NewLine()
        {
            if(_blankLineFlag)
            {
                AddBlankLine();
                _blankLineFlag = false;
            }
            else
            {
                _blankLineFlag = true;
            }
        }

        public void DeActiveButtons() { foreach (var com in _activeButtons) com.IsValid = false; _activeButtons.Clear(); }

        private class ButtonClickCommand : ICommand
        {
            private readonly Action<string> _action;

            public bool IsValid { get; set; }

            public ButtonClickCommand(Action<string> action) { _action = action; }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => IsValid;

            public void Execute(object parameter)
            {
                _action((string)parameter);
            }
        }
    }
}
