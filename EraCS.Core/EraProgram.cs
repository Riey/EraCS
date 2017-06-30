using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EraCS
{
    public enum ProgramStatus
    {
        Idle,
        Running,
        Waiting
    }

    public abstract class EraProgram<TVariable, TConsole>
        where TConsole : IEraConsole
    {
        protected readonly Stopwatch timer = new Stopwatch();
        protected InputRequest currentInputReq;

        public TConsole Console { get; }
        public TVariable VarData { get; private set; }

        public ProgramStatus Status =>
            timer.IsRunning
                ? IsWaiting
                    ? ProgramStatus.Waiting
                    : ProgramStatus.Running
                : ProgramStatus.Idle;

        public bool IsWaiting => currentInputReq != null;
        public long CurrentTime => timer.ElapsedMilliseconds;

        protected EraProgram(TConsole console, TVariable varData)
        {
            Console = console;
            VarData = varData;

            console.TextEntered += OnTextEntered;
        }

        // ReSharper disable once InconsistentNaming
        protected abstract void RunScriptAsync();

        private string _lastInputValue;
        private int _lastInputNumber;

        public void OnTextEntered(string value)
        {
            if (!IsWaiting) return;

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
                    _lastInputValue = value;
                    break;
            }

            currentInputReq = null;
        }

        public void Start()
        {
            timer.Start();

            RunScriptAsync();
        }

        protected virtual JsonSerializerSettings SerializerSettings { get; } =
            new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                ObjectCreationHandling = ObjectCreationHandling.Auto
            };

        public void Save(Stream output, bool leaveOpen)
        {
            using (var writer = new StreamWriter(output, Encoding.UTF8, 8192, leaveOpen))
                writer.Write(JsonConvert.SerializeObject(VarData, SerializerSettings));
        }

        public void Load(Stream input, bool leaveOpen)
        {
            using (var reader = new StreamReader(input, Encoding.UTF8, leaveOpen))
                VarData = JsonConvert.DeserializeObject<TVariable>(reader.ReadToEnd(), SerializerSettings);
        }

        protected const int WAIT_TIMEOUT = 150;

        public async Task WaitAnyKeyAsync()
        {
            await WaitAsync(new InputRequest(InputType.ANYKEY));
        }

        public async Task<int> WaitNumberAsync(bool isOneInput = false)
        {
            await WaitAsync(new InputRequest(InputType.INT, isOneInput));
            return _lastInputNumber;
        }

        public async Task<int> WaitNumberAsync(long endTime, int? defaultValue = null, bool isOneInput = false,
            Action<long> tickAction = null)
        {
            await WaitAsync(new InputRequest(InputType.INT, endTime, defaultValue?.ToString(), isOneInput), tickAction);
            return _lastInputNumber;
        }

        public async Task<string> WaitStringAsync(bool isOneInput = false)
        {
            await WaitAsync(new InputRequest(InputType.STR, isOneInput));
            return _lastInputValue;
        }

        public async Task<string> WaitStringAsync(long endTime, string defaultValue = null, bool isOneInput = false,
            Action<long> tickAction = null)
        {
            await WaitAsync(new InputRequest(InputType.STR, endTime, defaultValue, isOneInput), tickAction);
            return _lastInputValue;
        }

        public async Task WaitAsync(InputRequest req, Action<long> tickAction = null)
        {
            var target = req.EndTime + CurrentTime;
            currentInputReq = req;

            while (true)
            {
                await Task.Delay(WAIT_TIMEOUT);

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
