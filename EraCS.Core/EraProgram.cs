using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EraCS
{
    public enum ProgramStatus
    {
        Quit,
        Idle,
        Running,
        Waiting
    }

    public abstract class EraProgram<TConsole>
        where TConsole : IEraConsole
    {
        protected readonly object inputLock = new object();

        protected readonly Stopwatch timer = new Stopwatch();
        protected InputRequest currentInputReq;

        protected Task ScriptTask { get; private set; }

        public TConsole Console { get; }

        public ProgramStatus Status =>
            ScriptTask == null
                ? ProgramStatus.Idle
                : ScriptTask.IsCompleted
                    ? ProgramStatus.Quit
                    : IsWaiting
                        ? ProgramStatus.Waiting
                        : ProgramStatus.Running;

        public bool IsWaiting => currentInputReq != null;
        public long CurrentTime => timer.ElapsedMilliseconds;

        protected EraProgram(TConsole console)
        {
            Console = console;

            console.TextEntered += OnTextEntered;
            console.Clicked += () => OnTextEntered(null);
        }
        
        protected abstract Exception RunScript();

        private string _lastInputValue;
        private int _lastInputNumber;

        public void OnTextEntered(string value)
        {
            if (!IsWaiting) return;

            lock (inputLock)
            {
                if (currentInputReq.IsOneInput) value = value?[0].ToString();

                switch (currentInputReq.InputType)
                {
                    case InputType.ANYKEY:
                        _lastInputValue = null;
                        break;
                    case InputType.INT:
                        if (!int.TryParse(value, out var num)) return;
                        _lastInputValue = value;
                        _lastInputNumber = num;
                        break;
                    case InputType.STR:
                        if (value == null) return;
                        _lastInputValue = value;
                        break;
                    default:
                        throw new ArgumentException("Invalid InputType");
                }

                currentInputReq = null;
            }
        }

        public async Task Start()
        {
            timer.Start();

            ScriptTask = Task.Factory.StartNew(RunScript, TaskCreationOptions.LongRunning);

            await ManageScriptAsync();
        }

        private async Task ManageScriptAsync()
        {
            while(!ScriptTask.IsCompleted)
            {
                if(Console.NeedRedraw)
                {
                    Console.OnDrawRequested();
                }

                await Task.Delay(WAIT_TIMEOUT);
            }

            if (Console.NeedRedraw)
            {
                Console.OnDrawRequested();
            }

            ScriptTask = null;
            timer.Stop();
            timer.Reset();
        }

        protected const int WAIT_TIMEOUT = 50;

        public void WaitAnyKey()
        {
            Wait(new InputRequest(InputType.ANYKEY));
        }

        public int WaitNumber(bool isOneInput = false)
        {
            Wait(new InputRequest(InputType.INT, isOneInput));
            return _lastInputNumber;
        }

        public int WaitNumber(long endTime, int? defaultValue = null, bool isOneInput = false,
            Action<long> tickAction = null)
        {
            Wait(new InputRequest(InputType.INT, endTime, defaultValue?.ToString(), isOneInput), tickAction);
            return _lastInputNumber;
        }

        public string WaitString(bool isOneInput = false)
        {
            Wait(new InputRequest(InputType.STR, isOneInput));
            return _lastInputValue;
        }

        public string WaitString(long endTime, string defaultValue = null, bool isOneInput = false,
            Action<long> tickAction = null)
        {
            Wait(new InputRequest(InputType.STR, endTime, defaultValue, isOneInput), tickAction);
            return _lastInputValue;
        }

        public void Wait(InputRequest req, Action<long> tickAction = null)
        {
            long target = req.EndTime + CurrentTime;

            lock (inputLock)
            {
                currentInputReq = req;
            }

            while (true)
            {
                Task.Delay(WAIT_TIMEOUT).Wait();

                if (!IsWaiting) break;

                tickAction?.Invoke(target - CurrentTime);

                if (currentInputReq.HasTimeout && target <= CurrentTime)
                {
                    OnTextEntered(currentInputReq.DefaultValue);
                    break;
                }
            }

            Console.DeActiveButtons();
        }

    }
}
