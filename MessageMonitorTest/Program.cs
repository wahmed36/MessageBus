using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBus.DataBase.Mongo;
using MessageBus.Config;
using MessageBus.Interfaces;
using MessageBus.Message;
using MessageBus;
using MessagePublishingTest.Events;

namespace MessageMonitorTest
{
    public class MessageMonitor
    {
        private static ApplicationInfo AppInfo = new ApplicationInfo { ApplicationName = "ConsoleMoniter" };
        public static void Main(string[] args)
        {
            BusManager<FundsTransfer> FundsTransferBusManager = ObjectFactory.GetBusManager<FundsTransfer>();
            FundsTransferBusManager.Subscribe(new DummySubscriber<FundsTransfer>(), AppInfo);
            WaitForExit();            
        }

        private static void WaitForExit()
        {
            while(true)
            {
                Console.WriteLine("Do you want to exit?");
                string input = Console.ReadLine();
                if (input.IsNull()) continue;
                if ("exit".Equals(input.ToLower()))
                {
                    return;
                }else
                {
                    Console.WriteLine("Invalid command");
                }
            }
        }
    }
}
