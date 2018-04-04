using System.Linq;
using MongoDB.Driver;
using MessageBus.Config;
using System;

namespace MessageBus.DataBase.Mongo
{
    /// <summary>
    /// Responsible to maintain reading record for each queue and for each client (identfieid by Application info)
    /// </summary>
    public class ProcessedMessagesCollection
    {
        private IMongoDatabase mongoDB;
        private IMongoCollection<ProcessedMessageTrack> collection;

        public ProcessedMessagesCollection(IMongoDatabase mongoDB)
        {
            this.mongoDB = mongoDB;
            collection = mongoDB.GetCollection<ProcessedMessageTrack>(Constants.PROCESSED_MESSAGES_COLLECTION);
        }

        public void Save(ProcessedMessageTrack processedMessage)
        {
            ProcessedMessageTrack lastTrack = collection.Find(GetFilterDefinition(processedMessage)).FirstOrDefault();
            InsertOrUpdate(processedMessage,lastTrack);
            //collection.InsertOneAsync(processedMessage).Wait();
        }

        FilterDefinition<ProcessedMessageTrack> GetFilterDefinition(ProcessedMessageTrack processedMessage)
        {
            return Builders<ProcessedMessageTrack>.Filter.Where(x => x.MessageType == processedMessage.MessageType && x.ProcessingApplication == processedMessage.ProcessingApplication);            
        }
        
        private void InsertOrUpdate(ProcessedMessageTrack processedMessage, ProcessedMessageTrack lastMessage)
        {
            if(lastMessage.IsNull())
            {
                collection.InsertOneAsync(processedMessage).Wait();
            }
            else
            {
                collection.UpdateOne(GetFilterDefinition(processedMessage), UpdateDefinition(processedMessage) );
            }
        }

        UpdateDefinition<ProcessedMessageTrack> UpdateDefinition(ProcessedMessageTrack processedMessage)
        {
            return Builders<ProcessedMessageTrack>.Update.
                Set("EventID", processedMessage.EventID).
                Set("EventTime", processedMessage.EventTime).
                Set("MessageType", processedMessage.MessageType).
                Set("ProcessingApplication", processedMessage.ProcessingApplication);
        }

        public ProcessedMessageTrack GetTrackFor(ApplicationInfo applicationInfo)
        {
            if (applicationInfo.IsNull()) {
                throw new ArgumentNullException();
            }
            return collection.Find(x => x.ProcessingApplication.ApplicationName == applicationInfo.ApplicationName).FirstOrDefault();
        }
    }
}
