using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBus.Log;

namespace MessageBusTest
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void TestLogger()
        {
            Logger.Debug("Test message"+DateTime.Now);
        }
    }
}
