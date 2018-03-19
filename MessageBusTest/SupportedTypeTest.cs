using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBus.Config;
using System.Collections.Generic;

namespace MessageBusTest
{
    [TestClass]
    public class SupportedTypeTest
    {
        [TestMethod]
        public void TestAutoSupportedTypeCreated()
        {
            SupportedMessages supportedMessages = SupportedMessages.GetInstance();
            Assert.IsNotNull(supportedMessages);
            List<SupportedType> list = ConfigManager.LoadSupportedTypes();
            Assert.AreEqual(list.Count, supportedMessages.Count);
        }
    }
}
