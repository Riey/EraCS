using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EraCS.UI.EraConsole
{
    public enum LineAlignment
    {
        Left, Center, Right
    }

    public class EraConsole : IEraConsole
    {
        private bool _blankLineFlag;
        private readonly List<ConsoleButtonPart> _activeButtons = new List<ConsoleButtonPart>(100);
        private SKColor _consoleTextColor;
        private SKColor _consoleBackColor;
        private SKColor _consoleHighlightColor;
        private float _height;
        private IConsoleLinePart _lastCursorOnPart;


        public event TextEnteredHandler TextEntered;
        public event Action Clicked;
        public event Action DrawRequested;

        public object DataLock { get; } = new object();

        public bool NeedRedraw { get; protected set; }

        protected IList<IConsoleLine> Lines { get; }
        
        private ConsoleLine LastLine { get; set; }

        public LineAlignment Alignment { get; set; } = LineAlignment.Left;

        public float Height
        {
            get => _height;
            private set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
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

        public void SetTextColor(KnownColor c) => ConsoleTextColor = new SKColor((uint)c);
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
            lock (DataLock)
            {
                Height = Lines.Sum(l => l.Height);
                NeedRedraw = true;
            }
        }

        protected void AddBlankLine()
        {
            LastLine = new ConsoleLine(Alignment, LineHeight);
            Lines.Add(LastLine);
        }

        protected void AddPart(IConsoleLinePart part)
        {
            lock (DataLock)
            {
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

                NeedRedraw = true;
            }
        }

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

            float y = 0;

            lock (DataLock)
            {
                foreach (var line in Lines)
                {
                    line.DrawTo(c, y);
                    y += line.Height;
                }

                NeedRedraw = false;
            }
        }

        public virtual void OnDrawRequested()
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

            lock (DataLock)
            {
                var part = GetPart(x, y);

                if (_lastCursorOnPart != part)
                {
                    _lastCursorOnPart?.OnCursorExited();
                    part?.OnCursorEntered();
                    _lastCursorOnPart = part;
                }
                else
                {
                    part?.OnCursorOver(x, y);
                }

                NeedRedraw = true;
            }
        }

        public void OnClicked(float x, float y)
        {
            lock (DataLock)
            {
                OnCursorMoved(x, y);

                _lastCursorOnPart?.OnClicked(x, y);
                Clicked?.Invoke();
                NeedRedraw = true;
            }
        }

        public void OnTextEntered(string value)
        {
            lock (DataLock)
            {
                TextEntered?.Invoke(value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
