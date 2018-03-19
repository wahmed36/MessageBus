using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;


namespace MessageBus.Config
{
    [TestClass]
    public class ConfigManagerTest
    {
        [TestMethod]
        public void TestGetConfigSection()
        {
            List<SupportedType> result = ConfigManager.LoadSupportedTypes();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestLoadSupportedTypes()
        {
            List<SupportedType> result = ConfigManager.LoadSupportedTypes();
            Assert.IsTrue(result.Count > 0);
            foreach (var element in result)
            {
                StringAssert.Contains(element.Value, ".");
            }                        
        }

        [TestMethod]
        public void TestLoadStringParameters()
        {
            String param =  ConfigManager.GetStringParameter(Constants.MAX_COLLECTION_SIZE_INBYTES);
            Assert.IsNotNull(param);
        }

        [TestMethod]
        public void TestLoadIntParameters()
        {
            int value = ConfigManager.GetIntParameter(Constants.MAX_COLLECTION_SIZE_INBYTES);
            Assert.IsTrue(value > 0);
        }

        [TestMethod]
        public void TestLoadIntParameterForType()
        {
            SupportedMessages supportedMessages =  SupportedMessages.GetInstance();
            
            foreach(var supportedMessage in supportedMessages)
            {
                int result =  ConfigManager.GetIntParameter(supportedMessage.FullName, Constants.Max_NUMBER_OF_DOCUMENTS, 1000);
                Assert.IsTrue(result != 1000);
            }
        }

        [TestMethod]
        public void TestLoadInParamtersFailed()
        {
            int value = ConfigManager.GetIntParameter("No existant value", 1000);
            Assert.IsTrue(value == 1000);
        }
    }
}
