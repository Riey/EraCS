using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace EraCS.UI.EraConsole
{
    public sealed class EraConsole : BindableObject, IEraConsole
    {
        private readonly List<ConsoleButtonPart> _activeButtons = new List<ConsoleButtonPart>(100);
        private readonly StackLayout _stack;

        private bool _blankLineFlag = true;

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

        public static readonly BindableProperty ConsoleHighlightColorProperty =
            BindableProperty.Create(
                propertyName: nameof(ConsoleHighlightColor),
                returnType: typeof(Color),
                declaringType: typeof(EraConsole),
                defaultValue: Color.Yellow);

        public EraConsole()
        {
            _stack = new StackLayout() {VerticalOptions = LayoutOptions.EndAndExpand};
            _stack.SetBinding(StackLayout.BackgroundColorProperty, "ConsoleBackColor", BindingMode.OneWay);
        }

        private StackLayout LastLine => (StackLayout) _stack.Children[_stack.Children.Count - 1];

        public View View => _stack;

        public Color ConsoleTextColor
        {
            get => (Color) GetValue(ConsoleTextColorProperty);
            set => SetValueAndRaise(ConsoleTextColorProperty, value);
        }

        public Color ConsoleBackColor
        {
            get => (Color) GetValue(ConsoleBackColorProperty);
            set => SetValueAndRaise(ConsoleBackColorProperty, value);
        }

        public Color ConsoleHighlightColor
        {
            get => (Color) GetValue(ConsoleHighlightColorProperty);
            set => SetValueAndRaise(ConsoleHighlightColorProperty, value);
        }

        public bool SkipPrint { get; set; }
        public LayoutOptions Alignment { get; set; } = LayoutOptions.Start;

        public bool LastLineIsTemporary { get; set; }

        private void SetValueAndRaise(BindableProperty property, object value,
            [CallerMemberName] string propertyName = "")
        {
            SetValue(property, value);
            OnPropertyChanged(propertyName);
        }

        private void AddBlankLine()
        {
            var line = new StackLayout() {HorizontalOptions = Alignment, Orientation = StackOrientation.Horizontal};
            line.SetBinding(StackLayout.BackgroundColorProperty, "ConsoleBackColor", BindingMode.OneWay);
            _stack.Children.Add(line);
        }

        private void AddPart(ConsoleLinePart part)
        {
            if (SkipPrint) return;

            if (LastLineIsTemporary)
            {
                _stack.Children.RemoveAt(_stack.Children.Count - 1);
                LastLineIsTemporary = false;
            }

            if (_blankLineFlag || _stack.Children.Count == 0)
            {
                AddBlankLine();
                _blankLineFlag = false;
            }

            LastLine.Children.Add(part);
        }

        public void OnTextEntered(string value)
        {
            TextEntered?.Invoke(value);
        }

        public event TextEnteredHandler TextEntered;

        public void Print(string str) => AddPart(new ConsoleStringPart(ConsoleTextColor, str));

        public void PrintLine(string str)
        {
            Print(str);
            NewLine();
        }

        public void PrintButton(string text, string value)
        {
            var btn = new ConsoleButtonPart(text, value, ConsoleTextColor, ConsoleHighlightColor, OnTextEntered);
            _activeButtons.Add(btn);
            AddPart(btn);
        }

        public void NewLine()
        {
            if (LastLineIsTemporary)
            {
                _stack.Children.RemoveAt(_stack.Children.Count - 1);
                LastLineIsTemporary = false;
            }

            if (_blankLineFlag)
            {
                AddBlankLine();
                _blankLineFlag = false;
            }
            else
            {
                _blankLineFlag = true;
            }
        }

        public void DeleteLine(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _stack.Children.RemoveAt(_stack.Children.Count - 1);
            }

            LastLineIsTemporary = false;
        }

        public void DeActiveButtons()
        {
            foreach (var com in _activeButtons) com.Clickable = false;
            _activeButtons.Clear();
        }
    }
}
