using System.IO;

namespace EraCS.Core.Test.Program
{
    public class TestConfig : EraConfig
    {
        public TestConfig() : base(new MemoryStream()) { }
    }
}
