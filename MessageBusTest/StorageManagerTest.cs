using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBus.DataBase.Mongo;
using MessageBus.Config;
using MessageBus.Interfaces;
using MessageBus.Message;
using MessageBus;
using System.Threading;

namespace MessageBusTest
{
    [TestClass]
    public class StorageManagerTest
    {
        ApplicationInfo Publisher;
        ApplicationInfo Subscriber;
        [TestInitialize]
        public void SetupTest()
        {
            Publisher = new ApplicationInfo {ApplicationName = "UnitTest Publisher" };
            Subscriber = new ApplicationInfo {ApplicationName = "UnitTest Subscriber" };
        }

        [TestMethod]
        public void TestInitializationSuccessfull()
        {
            StorageManager storageManager = StorageManager.GetStorageManager();

            Assert.IsNotNull(storageManager);
        }

        [TestMethod]
        public void TestInitialization()
        {
            StorageManager manager = StorageManager.GetStorageManager();
            manager.initQueues(SupportedMessages.GetInstance());

            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("FailureTesting")]
        public void TestMessageFailure()
        {
            BusManager<string> manager = ObjectFactory.GetBusManager<string>();
            Response response = manager.SendMessage("test",Publisher);
            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public void TestMessageSuccess()
        {
            BusManager<Revenue> manager = ObjectFactory.GetBusManager<Revenue>();
            Response response = manager.SendMessage(new Revenue {ID=10, TotalRevenue=25.20 },Publisher);
            Assert.IsTrue(response.Success);
            Assert.IsNotNull(response.ErrorMessage);
        }

        [TestMethod]
        public void TestMonitoringSucess()
        {
            BusManager<Revenue> manager = ObjectFactory.GetBusManager<Revenue>();
            manager.Subscribe(new DummySubscriber<Revenue>(), Subscriber);
            Thread.Sleep(1000);
            Response response = manager.SendMessage(new Revenue { ID = 11, TotalRevenue = 25.20 }, Publisher);
            Assert.IsTrue(response.Success);
            Thread.Sleep(1000);
            response = manager.SendMessage(new Revenue { ID = 12, TotalRevenue = 25.20 }, Publisher);
            Assert.IsNotNull(response.ErrorMessage);
        }
    }
}
