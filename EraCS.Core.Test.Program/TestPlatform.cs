using System;
using System.Collections.Generic;
using EraCS.UI.EraConsole;
using Xamarin.Forms;

namespace EraCS.Core.Test.Program
{
    public class TestPlatform : IPlatform<TestVariableData, EraConsole, TestConfig>
    {
        private EraProgram<TestVariableData, EraConsole, TestConfig> _program;

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

            PrintWith("R", Color.Red);
            PrintWith("A", Color.OrangeRed);
            PrintWith("I", Color.Yellow);
            PrintWith("N", Color.Green);
            _program.Console.Print(" ");
            PrintWith("B", Color.Blue);
            PrintWith("O", Color.Indigo);
            PrintWith("W", Color.Purple);
            
            _program.Console.ConsoleTextColor = Color.White;

            _program.Console.NewLine();
            _program.Console.NewLine();

            _program.Console.PrintLine("Hello world!");
            _program.Console.PrintLine($"Current time: {_program.VarData.Time[0]}");
        }

        public void Initialize(EraProgram<TestVariableData, EraConsole, TestConfig> program)
        {
            _program = program;
        }
    }
}
