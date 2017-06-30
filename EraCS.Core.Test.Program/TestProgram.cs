using System;
using EraCS.UI.EraConsole;
using Xamarin.Forms;

namespace EraCS.Core.Test.Program
{
    public class TestProgram : EraProgram<TestVariableData, EraConsole>
    {
        public TestProgram() 
            : base(
                  new EraConsole
                  {
                      ConsoleTextColor = Color.FromRgb(196, 196, 196),
                      ConsoleBackColor = Color.Black,
                      ConsoleHighlightColor = Color.Yellow
                  } , 
                  new TestVariableData())
        {
        }

        public void SystemTitle()
        {
            VarData.Time = DateTime.Now;

            void PrintWith(string str, Color color)
            {
                Console.ConsoleTextColor = color;
                Console.Print(str);
            }

            PrintWith("R", Color.Red);
            PrintWith("A", Color.OrangeRed);
            PrintWith("I", Color.Yellow);
            PrintWith("N", Color.Green);
            Console.Print(" ");
            PrintWith("B", Color.Blue);
            PrintWith("O", Color.Indigo);
            PrintWith("W", Color.Purple);

            Console.ConsoleTextColor = Color.White;

            Console.NewLine();
            Console.NewLine();

            Console.PrintLine("Hello world!");
            Console.PrintLine($"Current time: {VarData.Time}");
        }

        protected override async void RunScriptAsync()
        {
            Console.Alignment = LayoutOptions.Center;
            SystemTitle();

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
