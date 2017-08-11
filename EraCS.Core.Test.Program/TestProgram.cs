using System;
using EraCS.UI.EraConsole;

namespace EraCS.Core.Test.Program
{

    public class TestProgram : EraProgram<TestVariableData, EraConsole>
    {
        public TestProgram() 
            : base(
                  new EraConsole
                  {
                      ConsoleTextColor = ColorTool.ToColor(KnownColor.White),
                      ConsoleBackColor = ColorTool.ToColor(KnownColor.Black),
                      ConsoleHighlightColor = ColorTool.ToColor(KnownColor.Yellow)
                  } , 
                  new TestVariableData())
        {
        }

        public void SystemTitle()
        {
            VarData.Time = DateTime.Now;

            void PrintWith(string str, KnownColor color)
            {
                Console.SetTextColor(color);
                Console.Print(str);
            }

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
            Console.NewLine();

            Console.PrintLine("Hello world!");
            Console.PrintLine($"Current time: {VarData.Time}");
        }

        protected override async void RunScriptAsync()
        {
            Console.Alignment = LineAlignment.Left;
            SystemTitle();

            Console.PrintLine("Wait anykey test");
            await WaitAnyKeyAsync();
            Console.NewLine();

            Console.PrintButton("Click Me!!", "ABC");
            Console.NewLine();

            Console.PrintLine("Please input");
            
            var input = await WaitStringAsync(6000, defaultValue: "Time over",
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
