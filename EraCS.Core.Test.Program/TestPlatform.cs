using System;
using System.Collections.Generic;
using Xamarin.Forms;

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

            void PrintWith(string str, Color color)
            {
                _program.Console.ConsoleTextColor = color;
                _program.Console.Print(str);
            }

            PrintWith("F", Color.Red);
            PrintWith("U", Color.OrangeRed);
            PrintWith("C", Color.Yellow);
            PrintWith("K", Color.Green);
            _program.Console.Print(" ");
            PrintWith("Y", Color.Blue);
            PrintWith("O", Color.Indigo);
            PrintWith("U", Color.Purple);
            
            _program.Console.ConsoleTextColor = Color.White;

            _program.Console.NewLine();
            _program.Console.NewLine();

            _program.Console.PrintLine("Hello world!");
            _program.Console.PrintLine($"Current time: {_program.VarData.Time[0]}");
        }

        public void Initialize(EraProgram<TestVariableData, TestConfig> program)
        {
            _program = program;
        }
    }
}
