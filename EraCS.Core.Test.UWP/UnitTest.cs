
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EraCS.Core.Test.Program;
using System.IO;

namespace EraCS.Core.Test.UWP
{
    [TestClass]
    public class VariableTest
    {
        private readonly TestProgram _program;

        public VariableTest()
        {
            _program = new TestProgram();
        }

        [TestMethod]
        public void SaveLoadTest()
        {
            var ms = new MemoryStream();
            var firstTime = DateTime.Now;
            _program.VarData.Time[0] = firstTime;

            _program.Save(ms, true);
            var lastTime = firstTime + TimeSpan.FromDays(1);
            _program.VarData.Time[0] = lastTime;

            Assert.AreEqual(lastTime, _program.VarData.Time[0]);

            ms.Seek(0, SeekOrigin.Begin);
            _program.Load(ms, true);
            ms.Dispose();

            Assert.AreEqual(firstTime, _program.VarData.Time[0]);
        }
    }
}
