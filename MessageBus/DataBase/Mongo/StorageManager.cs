using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MessageBus.Config;
using MessageBus.Message;

/// <summary>
/// This class wraps a Mongo Connection to the messaging database database
/// </summary>
namespace MessageBus.DataBase.Mongo
{
    /// <summary>
    /// Responsible to handle all Mongo interactions and wrap database connections
    /// </summary>
    public class StorageManager
    {
        private MongoClient client;
        private IMongoDatabase MongoDB;
        private StorageManager() { }
        private Dictionary<string,IMongoCollection<BsonDocument>> collections =  new Dictionary<string, IMongoCollection<BsonDocument>>();
        //List of all existing collections in MongoDB. This information will be used to create new collection or resuse existing one

        private List<string> CollectionNames = new List<string>();
        const int CURSOR_MAX_WAIT_TIME = 5;
        public static StorageManager GetStorageManager()
        {   
            StorageManager storage = new StorageManager();
            storage.client = new MongoClient(ConfigManager.GetConnectionString(Constants.MONGODB_CONNECTION_NAME));
            string dbName = ConfigManager.GetAppSetting(Constants.MONGODB_NAME);
            storage.MongoDB = storage.client.GetDatabase(dbName);
            storage.LoadAllExistingCollectionNames();
            return storage;
        }

        /// <summary>
        /// @todo - collection configuration is needed to be added later
        /// </summary>
        /// <param name="mappings"></param>
        public void initQueues(SupportedMessages mappings)
        {
            foreach(var mapping in mappings)
            {
                string collectionName = DBUtil.GetCollectionName(mapping);
                
                if (!IsCollectionExists(collectionName))
                {
                    MongoDB.CreateCollection(collectionName, new CreateCollectionOptions { Capped = true,
                        MaxDocuments = ConfigManager.GetIntParameter(mapping.GetType().FullName, Constants.Max_NUMBER_OF_DOCUMENTS, 1000),
                        MaxSize = ConfigManager.GetIntParameter(mapping.GetType().FullName, Constants.MAX_COLLECTION_SIZE_INBYTES, 1024 * 1024) });
                }

                IMongoCollection<BsonDocument> collection = MongoDB.GetCollection<BsonDocument>(collectionName);
                collections.Add(collectionName, collection);
            }
        }

        private void LoadAllExistingCollectionNames()
        {
            foreach(BsonDocument collection in this.MongoDB.ListCollectionsAsync().Result.ToListAsync<BsonDocument>().Result)
            {
                CollectionNames.Add(collection["name"].AsString);
            }
            
        }

        private bool IsCollectionExists(string collectionName)
        {
            bool result = false;
            CollectionNames.ForEach(x =>{ if (x == collectionName) result = true;  });
            return result;
        }

        public bool SaveMessage<t>(BusMessage<t> message) 
        {
            bool result = false;
            IMongoCollection<BusMessage<t>> collection = GetMongoCollection<t>();

            if(isValidCollection(collection))
            {
                collection.InsertOneAsync(message).Wait();

                result= true;
            }
            return result;            
        }

        private bool isValidCollection<t>(IMongoCollection<t> collection)
        {
           return collection != null? true: false;
        }

        /// <summary>
        /// Method will save list of messages if supported by message bus.
        /// It will ignore the publishing if given list is not supported
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="messages"></param>
        public void SaveMessages<t>(List<BusMessage<t>> messages)
        {
            IMongoCollection<BusMessage<t>> collection = GetMongoCollection<t>();

            if(isValidCollection<BusMessage<t>>(collection))
            {
                collection.InsertManyAsync(messages).Wait();
            }   
        }

        private IAsyncCursor<BusMessage<t>> GetCursor<t>(FilterDefinition<BusMessage<t>> filterDefinition)
        {            
            IMongoCollection<BusMessage<t>> mongoCollection = GetMongoCollection<t>();            
            
            IAsyncCursor<BusMessage<t>> cursor=  mongoCollection.FindSync<BusMessage<t>>(filterDefinition, GetFindOptions<t>());
            return cursor;
        }

        public void RegisterMessageListener<t>(Action<IAsyncCursor<BusMessage<t>>, StorageManager> callback, FilterDefinition<BusMessage<t>> filterDefinition)
        {
            callback(GetCursor<t>(filterDefinition),this);
        }

        private FindOptions<BusMessage<t>> GetFindOptions<t>()
        {
            return new FindOptions<BusMessage<t>> {  CursorType = CursorType.Tailable,
                MaxAwaitTime = TimeSpan.FromSeconds(CURSOR_MAX_WAIT_TIME),
                NoCursorTimeout =true};
        }


        private IMongoCollection<BusMessage<t>> GetMongoCollection<t>() 
        {
            if(SupportedMessages.GetInstance().IsTypeSupported(typeof(t)))
            {
                string collectionName = DBUtil.GetCollectionName(typeof(t));
                return MongoDB.GetCollection<BusMessage<t>>(collectionName);                
            }
            else
            {
                return null;
            }            
        }

        public void SaveProcessedMessageTrack(ProcessedMessageTrack processedMessage)
        {
            new ProcessedMessagesCollection(this.MongoDB).Save(processedMessage);
        }

        public ProcessedMessageTrack GetTrackFor(ApplicationInfo listenerApplication)
        {
            return new ProcessedMessagesCollection(this.MongoDB).GetTrackFor(listenerApplication);
        }

        public bool IsActive()
        {
            return this.MongoDB.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
        }
    }
}
