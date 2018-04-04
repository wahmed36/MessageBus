using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBus.Interfaces;
using MessageBus.Message;
using MessageBus.Config;
using MessageBus.DataBase.Mongo;
using MessageBus.Log;

namespace MessageBus.Impl
{
    /// <summary>
    /// Responsible to implement message bus messages publishing and subscriber control all subscribers
    /// </summary>
    /// <typeparam name="t"></typeparam>
    class BusManagerImpl<t> : BusManager<t>
    {
        private static BusManagerImpl<t> manager = new BusManagerImpl<t>();
        
        private StorageManager storeManager;

        private Dictionary<string, MessageMonitor<t>> subscribers= new Dictionary<string,MessageMonitor<t>>();

        static BusManagerImpl()
        {            
            manager.Init();
        }

        public static BusManager<t> GetInstance()
        {   
            return manager;            
        }

        /// <summary>
        /// Steps of initiation are
        /// 1- open DB Connection
        /// 2- Create all required collections, if not existing
        /// 3- Open cursor with active records
        /// </summary>
        private void Init()
        {
            Logger.Debug("Bus manager initialized");
            storeManager = MessageBus.DataBase.Mongo.StorageManager.GetStorageManager();
            storeManager.initQueues(SupportedMessages.GetInstance());            
        }

        /// <summary>
        /// Publish the message on the message bus, if appropriate mapping is found in the
        /// message mapping file
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="message"></param>
        /// <param name="senderApplication"></param>
        public Response SendMessage(t message, ApplicationInfo senderApplication, TimeSpan messageTTL = default(TimeSpan))
        {
            Logger.Debug(string.Format("{0} published a message on {1}. Message = {2}",senderApplication.ApplicationName,DateTime.Now,message.ToJSON()));
            Response defaultResonse = DefaultResponse();

            if (!SupportedMessages.GetInstance().IsTypeSupported(message.GetType()))
            {
                Logger.Debug("default response returned. response = "+defaultResonse.ToJSON());
                return defaultResonse;
            }

            if (storeManager.SaveMessage<t>(new BusMessage<t>(senderApplication.ApplicationName, message,messageTTL)))
            {
                defaultResonse.Success = true;
                defaultResonse.ErrorMessage = "Message published successfully";
                defaultResonse.ErrorCode = 0;
            }
            else
            {
                defaultResonse.ErrorMessage = "Failed to publish message";
            }

            Logger.Debug(defaultResonse.ToJSON());
            return defaultResonse;

        }

        private Response DefaultResponse()
        {
            return new Response
            {
                Success = false,
                ErrorMessage = "Message type is not supported",
                ErrorCode = 1
            };
        }

        public void Subscribe(Subscriber<t> subscriber, ApplicationInfo listenerApplication)
        {
            if(subscriber.IsNull() || listenerApplication.IsNull())
            {
                throw new ArgumentNullException();
            }

            /**Here assumption is that only subscriber will exist one app context for one type of message.
            if double entry is tried that system will ignore the subscription attempt. It is the 
            responsiblity of the caller to ensure that double subscription doesnt happen
             */

            if(!this.subscribers.ContainsKey(typeof(t).FullName))
            {
                MessageMonitor<t> messageMonitors = new MessageMonitor<t>(subscriber, listenerApplication);
                this.subscribers.Add(typeof(t).FullName, messageMonitors);
            }            
        }
    }    
}
