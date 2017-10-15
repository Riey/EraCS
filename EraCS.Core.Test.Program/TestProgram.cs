using System;
using EraCS.UI.EraConsole;

namespace EraCS.Core.Test.Program
{

    public class TestProgram : EraProgram<EraConsole, TestVariableData>
    {
        public TestProgram() : base(new EraConsole(), new TestVariableData())
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

        protected override async void RunScriptAsync()
        {
            Console.Alignment = LineAlignment.Left;
            SystemTitle();

            Console.PrintLine("Wait anykey test");
            await WaitAnyKeyAsync();
            Console.NewLine();

            Console.Alignment = LineAlignment.Center;

            Console.PrintButton("Click Me!!", "ABC");
            Console.NewLine();

            Console.PrintLine("Please input");

            string input = await WaitStringAsync(6000, defaultValue: "Time over",
                tickAction: (left) =>
                {
                    Console.PrintLine($"Time {6000 - left}/6000");
                    Console.LastLineIsTemporary = true;
                });
            
            Console.DeleteLine(2);
            Console.NewLine();
            Console.NewLine();
            Console.PrintLine($"You wrote: {input}");
        }
    }
}
