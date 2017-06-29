using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

    public abstract class EraProgram<TVariable, TConsole, TConfig>
        where TConsole : IEraConsole where TConfig : EraConfig
    {
        private readonly Stopwatch _timer = new Stopwatch();
        protected IReadOnlyDictionary<string, Delegate> methods;
        protected InputRequest currentInputReq;

        public TConsole Console { get; }
        public TVariable VarData { get; private set; }
        public TConfig Config { get; }

        public ProgramStatus Status =>
            _timer.IsRunning
                ? currentInputReq == null
                    ? ProgramStatus.Waiting
                    : ProgramStatus.Running
                : ProgramStatus.Idle;

        public bool IsWaiting => currentInputReq != null;
        public long CurrentTime => _timer.ElapsedMilliseconds;

        protected EraProgram(TConsole console, TVariable varData, TConfig config)
        {
            Console = console;
            VarData = varData;
            Config = config;

            console.ConsoleBackColor = config.BackColor;
            console.ConsoleTextColor = config.TextColor;

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

        public void Start(params IPlatform<TVariable, TConsole, TConfig>[] platforms)
        {
            foreach (var p in platforms) p.Initialize(this);

            methods =
                platforms
                    .SelectMany(p => p.Methods)
                    .ToDictionary(p => p.Key, p => p.Value);

            _timer.Start();

            RunScriptAsync();
        }

        public object Call(string name, params object[] args)
        {
            if (methods.TryGetValue(name, out var func))
            {
                return func.DynamicInvoke(args);
            }

            throw new ArgumentException();
        }

        public void Save(Stream output, bool leaveOpen)
        {
            using (var writer = new StreamWriter(output, Encoding.UTF8, 8192, leaveOpen))
                writer.Write(JsonConvert.SerializeObject(VarData, Formatting.Indented));
        }

        public void Load(Stream input, bool leaveOpen)
        {
            using (var reader = new StreamReader(input, Encoding.UTF8, leaveOpen))
                VarData = JsonConvert.DeserializeObject<TVariable>(reader.ReadToEnd());
        }

        public T Call<T>(string name, params object[] args) => (T) Call(name, args);

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
