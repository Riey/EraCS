using System;
using System.Collections.Generic;
using EraCS.Variable;

namespace EraCS
{
    public interface IPlatform<TVariable, TConsole, TConfig> where TVariable : VariableDataBase where TConsole : IEraConsole where TConfig : EraConfig
    {
        void Initialize(EraProgram<TVariable, TConsole, TConfig> program);

        IReadOnlyDictionary<string, Delegate> Methods { get; }

    }
}