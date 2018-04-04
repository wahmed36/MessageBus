using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBus.Interfaces;
using MessageBus.Log;

namespace MessageBus.Message
{
    /// <summary>
    /// Sample subscribers
    /// </summary>
    /// <typeparam name="t"></typeparam>
    public class DummySubscriber<t> : Subscriber<t>
    {
        public List<string> InterestedInSources { get; set; }

        public void OnError(Exception exception)
        {
            Logger.Error(this,exception);
        }

        public void OnEvent(t message)
        {
            Console.WriteLine(message.ToJSON());
        }

        public void OnEvents(List<t> messages)
        {
            foreach(var message in messages)
            {
                OnEvent(message);
            }
        }
    }
}
