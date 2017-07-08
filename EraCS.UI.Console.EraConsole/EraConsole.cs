using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EraCS.UI.EraConsole.Annotations;
using SkiaSharp;

namespace EraCS.UI.EraConsole
{
    public enum LineAlignment
    {
        Left, Center, Right
    }

    public static class ColorTool
    {
        public static SKColor ToColor(KnownColor c) => new SKColor((uint)c);
        public static SKColor ToColor(uint rgba) => new SKColor(rgba);
    }

    public class EraConsole : IEraConsole
    {
        private bool _blankLineFlag;
        private readonly List<ConsoleButtonPart> _activeButtons = new List<ConsoleButtonPart>(100);
        private SKColor _consoleTextColor;
        private SKColor _consoleBackColor;
        private SKColor _consoleHighlightColor;
        private float _height;
        private ConsoleButtonPart _lastCursorOnBtn;

        public IList<IConsoleLine> Lines { get; }
        
        private ConsoleLine LastLine { get; set; }

        public bool SkipPrint { get; set; }
        public LineAlignment Alignment { get; set; } = LineAlignment.Left;

        public float Height
        {
            get => _height;
            private set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public bool LastLineIsTemporary { get; set; }
        public SKTypeface Typeface { get; set; } = SKTypeface.FromFamilyName("MS Gothic");

        public EraConsole()
        {
            var lines = new ObservableCollection<IConsoleLine>();
            Lines = lines;

            lines.CollectionChanged += LinesOnCollectionChanged;
        }

        private void LinesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            Height = Lines.Count * LineHeight;
            OnDrawRequested();
        }

        private void AddBlankLine()
        {
            LastLine = new ConsoleLine(LineHeight);
            Lines.Add(LastLine);
        }

        private void AddPart(IConsoleLinePart part)
        {
            if (SkipPrint) return;

            if (LastLineIsTemporary)
            {
                Lines.RemoveAt(Lines.Count - 1);
                LastLineIsTemporary = false;
            }

            if (_blankLineFlag || Lines.Count == 0)
            {
                AddBlankLine();
                _blankLineFlag = false;
            }

            LastLine.Parts.Add(part);
        }



        public SKColor ConsoleTextColor
        {
            get => _consoleTextColor;
            set
            {
                if (value.Equals(_consoleTextColor)) return;
                _consoleTextColor = value;
                OnPropertyChanged();
            }
        }

        public void SetTextColor(KnownColor c) => ConsoleTextColor = new SKColor((uint) c);
        public void SetTextColor(uint c) => ConsoleTextColor = new SKColor(c);

        public SKColor ConsoleBackColor
        {
            get => _consoleBackColor;
            set
            {
                if (value.Equals(_consoleBackColor)) return;
                _consoleBackColor = value;
                OnPropertyChanged();
            }
        }

        public void SetBackColor(KnownColor c) => ConsoleBackColor = new SKColor((uint)c);
        public void SetBackColor(uint c) => ConsoleBackColor = new SKColor(c);

        public SKColor ConsoleHighlightColor
        {
            get => _consoleHighlightColor;
            set
            {
                if (value.Equals(_consoleHighlightColor)) return;
                _consoleHighlightColor = value;
                OnPropertyChanged();
            }
        }

        public void SetHighlightColor(KnownColor c) => ConsoleHighlightColor = new SKColor((uint)c);
        public void SetHighlightColor(uint c) => ConsoleHighlightColor = new SKColor(c);

        public float LineHeight { get; set; } = 30;
        public float TextSize { get; set; } = 15;

        public event TextEnteredHandler TextEntered;

        public void Print(string str) => AddPart(new ConsoleStringPart(ConsoleTextColor, str, TextSize, Typeface));

        public void PrintLine(string str)
        {
            Print(str);
            NewLine();
        }

        public void PrintButton(string text, string value)
        {
            var btn = new ConsoleButtonPart(text, TextSize, Typeface, value, ConsoleTextColor, ConsoleHighlightColor, OnTextEntered);
            _activeButtons.Add(btn);
            AddPart(btn);
        }

        public void NewLine()
        {
            if (LastLineIsTemporary)
            {
                Lines.RemoveAt(Lines.Count - 1);
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
                Lines.RemoveAt(Lines.Count - 1);
            }

            LastLineIsTemporary = false;
        }

        public void DeActiveButtons()
        {
            foreach (var com in _activeButtons) com.Clickable = false;
            _activeButtons.Clear();
        }

        public virtual void Draw(SKCanvas c)
        {
            c.DrawColor(ConsoleBackColor);

            float y = LineHeight / 2;

            foreach (var line in Lines)
            {
                line.DrawTo(c, y);
                y += LineHeight;
            }
        }

        public event Action DrawRequested;

        protected virtual void OnDrawRequested()
        {
            DrawRequested?.Invoke();
        }

        private IConsoleLinePart GetPart(float x, float y)
        {
            foreach (var line in Lines)
            {
                y -= line.Height;

                if (y > 0)
                {
                    continue;
                }

                return line.GetPart(x);
            }

            return null;
        }

        public void OnCursorMoved(float x, float y)
        {
            if(x < 0) throw new ArgumentOutOfRangeException(nameof(x));
            if(y < 0) throw new ArgumentOutOfRangeException(nameof(y));

            if (_lastCursorOnBtn != null)
                _lastCursorOnBtn.CursorOn = false;

            if (GetPart(x, y) is ConsoleButtonPart btnPart)
            {
                btnPart.CursorOn = true;
                _lastCursorOnBtn = btnPart;
            }
        }

        public void OnClicked(float x, float y)
        {
            OnCursorMoved(x, y);

            _lastCursorOnBtn?.ClickAction();
        }

        public void OnTextEntered(string value)
        {
            TextEntered?.Invoke(value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
