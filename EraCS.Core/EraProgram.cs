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


        public Task WorkerTask { get; private set; }
        public bool IsRunning => WorkerTask?.Status == TaskStatus.Running;
        public bool IsWaiting => _currentInputReq != null;
        public bool IsWorkerTask => WorkerTask?.Id == Task.CurrentId;
        public long CurrentTime => _timer.ElapsedMilliseconds;

        public EraProgram(EraConsole console, TVariable varData, TConfig config)
        {
            Console = console;
            VarData = varData;
            Config = config;

            console.TextEntered += OnTextEntered;
        }

        protected abstract void RunScript();

        public void OnTextEntered(string value)
        {

            lock (_inputLock)
            {
                if (!IsWaiting) return;

                switch (_currentInputReq.InputType)
                {
                    case InputType.ANYKEY:
                        break;
                    case InputType.INT:
                        if (!int.TryParse(value, out var num)) return;
                        VarData.Result[0] = num;
                        break;
                    case InputType.STR:
                        VarData.ResultS[0] = value;
                        break;
                }

                _currentInputReq = null;

                Monitor.PulseAll(_inputLock);
            }
        }

        public void Start(params IPlatform<TVariable, TConfig>[] platforms)
        {
            if (IsRunning) throw new InvalidOperationException();

            foreach (var p in platforms) p.Initialize(this);

            _methods =
                platforms
                .SelectMany(p => p.Methods)
                .ToDictionary(p => p.Key, p => p.Value);

            WorkerTask = new Task(RunScript, TaskCreationOptions.LongRunning);
            WorkerTask.Start();
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
        public void Wait(InputRequest req)
        {
            Monitor.Enter(_inputLock);
            {
                _currentInputReq = req;

                while (IsWaiting)
                {
                    if (Monitor.Wait(_inputLock, WAIT_TIMEOUT)) break;

                    if (req.HasTimeout && req.EndTime < CurrentTime)
                    {
                        Monitor.Exit(_inputLock);
                        OnTextEntered(req.DefaultValue);
                        return;
                    }
                }
            }
            Monitor.Exit(_inputLock);

        }

    }
}
