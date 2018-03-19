using System;
using MongoDB.Bson;

namespace MessageBus.DataBase.Mongo
{
    /// <summary>
    /// This class will use to record last processed record by 
    /// </summary>
    public class ProcessedMessageTrack
    {
        public ObjectId _id { set; get; }
        //type of message processed
        public String MessageType { set; get; }
        //processed by
        public ApplicationInfo ProcessingApplication { set; get; }
        //event id of the processed event
        public ObjectId EventID { set; get; }
        //Creation time of the event that was processed and recordded
        public DateTime EventTime { set; get; }

        public ProcessedMessageTrack() { }
    }
}
