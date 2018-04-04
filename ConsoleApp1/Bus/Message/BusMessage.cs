using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MessageBus.Message
{
    /// <summary>
    /// Responsible to represent internal message handling mechanism. Every published message is converted into BusMessage for that type
    /// </summary>
    /// <typeparam name="t"></typeparam>
    public class BusMessage<t>
    {
        public ObjectId _id { get; set; }
        public t Message { get; set; }
        public string SenderApplicationName { get; set; }
        public bool DeleteOnRead { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? TTL { get; set; }

        public BusMessage(string sender, t message, TimeSpan messageTTL)
        {            
            this.Message = message;
            this.CreationTime = DateTime.Now;
            this.SenderApplicationName = sender;
            this.DeleteOnRead = false;

            if(messageTTL != default(TimeSpan))
            {
                TTL = DateTime.Now.Add(messageTTL);
            }
        }

        public BusMessage()
        {
        }
    }
}
