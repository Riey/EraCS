using System;
using EraCS.UI.EraConsole;

namespace EraCS.Core.Test.Program
{

    public class TestProgram : EraProgram<EraConsole>
    {
        public TestVariableData VarData { get; private set; } = new TestVariableData();

        public TestProgram() : base(new EraConsole())
        {
            Console.SetBackColor(KnownColor.Black);
            Console.SetHighlightColor(KnownColor.Yellow);
            Console.SetTextColor(KnownColor.White);
        }

        public void SystemTitle()
        {
            VarData.Time = DateTime.Now;

            void PrintWith(string str, KnownColor color)
            {
                Console.SetTextColor(color);
                Console.Print(str);
            }

            Console.Alignment = LineAlignment.Center;

            PrintWith("R", KnownColor.Red);
            PrintWith("A", KnownColor.OrangeRed);
            PrintWith("I", KnownColor.Yellow);
            PrintWith("N", KnownColor.Green);
            Console.Print(" ");
            PrintWith("B", KnownColor.Blue);
            PrintWith("O", KnownColor.Indigo);
            PrintWith("W", KnownColor.Purple);

            Console.SetTextColor(KnownColor.White);

            Console.NewLine();
            Console.Alignment = LineAlignment.Left;
            Console.NewLine();
            Console.PrintLine("Hello world!");

            Console.Alignment = LineAlignment.Right;
            Console.PrintLine($"Current time: {VarData.Time}");
        }

        protected override Exception RunScript()
        {
            Console.Alignment = LineAlignment.Left;
            SystemTitle();

            Console.PrintLine("Wait anykey test");
            WaitAnyKey();
            Console.NewLine();

            Console.Alignment = LineAlignment.Center;

            Console.PrintButton("Click Me!!", "ABC");
            Console.NewLine();

            Console.PrintLine("Please input");

            string input = WaitString();

            Console.DeleteLine(2);
            Console.NewLine();
            Console.NewLine();
            Console.PrintLine($"You wrote: {input}");

            return null;
        }
    }
}
