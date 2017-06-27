using EraCS.Console;
using EraCS.Variable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EraCS
{
    public interface IPlatform<TVariable, TConfig> where TVariable : VariableDataBase where TConfig : EraConfig
    {
        void Initialize(EraProgram<TVariable, TConfig> program);

        IReadOnlyDictionary<string, Delegate> Methods { get; }

    }

    public enum ProgramStatus
    {
        Idle,
        Running,
        Waiting
    } 

    public abstract class EraProgram<TVariable, TConfig>
        where TVariable : VariableDataBase where TConfig : EraConfig
    {
        private readonly Stopwatch _timer = new Stopwatch();
        protected IReadOnlyDictionary<string, Delegate> _methods;
        protected InputRequest _currentInputReq;
        protected object _inputLock = new object();

        public EraConsole Console { get; }
        public TVariable VarData { get; }
        public TConfig Config { get; }
        public ProgramStatus Status;
        
        public bool IsWaiting => _currentInputReq != null;
        public long CurrentTime => _timer.ElapsedMilliseconds;

        public EraProgram(EraConsole console, TVariable varData, TConfig config)
        {
            Console = console;
            VarData = varData;
            Config = config;

            console.ConsoleBackColor = config.BackColor;
            console.ConsoleTextColor = config.TextColor;

            console.TextEntered += OnTextEntered;
        }

        protected abstract void RunScriptAsync();

        private string _lastInputValue = null;

        public void OnTextEntered(string value)
        {
            if (!IsWaiting) return;

            Status = ProgramStatus.Waiting;

            switch (_currentInputReq.InputType)
            {
                case InputType.ANYKEY:
                    _lastInputValue = null;
                    break;
                case InputType.INT:
                    if (!int.TryParse(value, out var num)) return;
                    _lastInputValue = value;
                    break;
                case InputType.STR:
                    _lastInputValue = value;
                    break;
            }

            _currentInputReq = null;
        }

        public void Start(params IPlatform<TVariable, TConfig>[] platforms)
        {
            foreach (var p in platforms) p.Initialize(this);

            _methods =
                platforms
                .SelectMany(p => p.Methods)
                .ToDictionary(p => p.Key, p => p.Value);

            RunScriptAsync();
        }

        public object Call(string name, params object[] args)
        {
            if (_methods.TryGetValue(name, out var func))
            {
                return func.DynamicInvoke(args);
            }

            throw new ArgumentException();
        }

        public T Call<T>(string name, params object[] args) => (T)Call(name, args);

        protected const int WAIT_TIMEOUT = 250;

        public async Task<string> WaitAsync(InputRequest req)
        {
            _currentInputReq = req;

            while (IsWaiting)
            {
                await Task.Delay(WAIT_TIMEOUT);

                if (req.HasTimeout && req.EndTime < CurrentTime)
                {
                    OnTextEntered(req.DefaultValue);
                    break;
                }
            }

            return _lastInputValue;
        }

    }
}
