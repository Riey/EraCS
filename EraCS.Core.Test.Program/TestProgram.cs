using System.Collections.Generic;
using System.Linq;
using System.Text;
using EraCS.Console;

namespace EraCS.Core.Test.Program
{
    public class TestProgram : EraProgram<TestVariableData, TestConfig>
    {
        public TestProgram() : base(new EraConsole(), new TestVariableData(), new TestConfig())
        {
        }

        protected override async void RunScriptAsync()
        {
            Call("SystemTitle");
            var input = await WaitAsync(new InputRequest(InputType.STR));

            Console.NewLine();
            Console.NewLine();
            Console.PrintLine($"You wrote: {input}");
        }
    }
}
