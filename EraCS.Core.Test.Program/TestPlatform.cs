using System;
using System.Collections.Generic;

namespace EraCS.Core.Test.Program
{
    public class TestPlatform : IPlatform<TestVariableData, TestConfig>
    {
        private EraProgram<TestVariableData, TestConfig> _program;

        public IReadOnlyDictionary<string, Delegate> Methods =>
            new Dictionary<string, Delegate>()
            {
                {"SystemTitle", (Action)SystemTitle }
            };

        public void SystemTitle()
        {
            _program.VarData.Time[0] = DateTime.Now;
            _program.Console.PrintLine("Hello world!");
            _program.Console.PrintLine($"Current time: {_program.VarData.Time[0]}");
        }

        public void Initialize(EraProgram<TestVariableData, TestConfig> program)
        {
            _program = program;
        }
    }
}
