using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBus.Message;
using MessageBus.DataBase;

namespace MessageBusTest
{
    [TestClass]
    public class DBUtilTest
    {
        [TestMethod]
        public void TestGetCollectionName()
        {
            String name = DBUtil.GetCollectionName(typeof(Revenue));
            Assert.IsNotNull(name);
            StringAssert.Contains(name,"_");

            string name2 = DBUtil.GetCollectionName(typeof(Revenue));
            Assert.AreEqual(name, name2);
        }
    }
}
