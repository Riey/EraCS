using System.IO;
using Xamarin.Forms;

namespace EraCS.Core.Test.Program
{
    public class TestConfig : EraConfig
    {
        public override Color TextColor => Color.White;
        public override Color BackColor => Color.Black;
        public override double TextSize => 20;
    }
}
