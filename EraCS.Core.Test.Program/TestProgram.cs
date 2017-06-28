using EraCS.UI.EraConsole;
using Xamarin.Forms;

namespace EraCS.Core.Test.Program
{
    public class TestProgram : EraProgram<TestVariableData, EraConsole, TestConfig>
    {
        public TestProgram() : base(new EraConsole(), new TestVariableData(), new TestConfig())
        {
        }

        protected override async void RunScriptAsync()
        {
            Console.Alignment = LayoutOptions.Center;
            Call("SystemTitle");

            Console.PrintButton("Click Me!!", "ABC");
            Console.NewLine();

            Console.PrintLine("Please input");
            
            var input = await WaitStringAsync(6000, defaultValue: "FUXX You",
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
