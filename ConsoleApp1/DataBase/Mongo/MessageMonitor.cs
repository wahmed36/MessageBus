using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBus.Interfaces;
using MessageBus.DataBase.Mongo;
using MessageBus.Message;
using MongoDB.Driver;
using System.Threading;
using MessageBus.Log;

namespace MessageBus.DataBase.Mongo
{
    /// <summary>
    /// Responible to monitor queue and fire subscribers event whenever a new message is published
    /// </summary>
    /// <typeparam name="t"></typeparam>
    public class MessageMonitor<t>
    {
        private Subscriber<t> subscriber;
        private ApplicationInfo listenerApplication;
        private ProcessedMessageTrack messageTrack;       

        public MessageMonitor(Subscriber<t> subscriber, ApplicationInfo listenerApplication)
        {
            this.subscriber = subscriber;
            this.listenerApplication = listenerApplication;
            ThreadStart threadStart = new ThreadStart(() => { startMonitoring(); });
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        private void startMonitoring()
        {
            StorageManager storageManager = StorageManager.GetStorageManager();
            int errorCounter = 0;

            while (errorCounter < 3)
            {
                try
                {
                    storageManager.RegisterMessageListener<t>(MonitorQueue, GetMessagesFilter(storageManager));
                    errorCounter = 0;
                }catch(Exception exception)
                {
                    HandleError(ref storageManager, ref errorCounter, exception);
                }
            }

            Log.Logger.Debug(string.Format("{0} class is stopping monitoring due to persistent connection error",typeof(t).FullName));
        }

        private void HandleError(ref StorageManager storageManager, ref int errorCounter, Exception exception)
        {
            try
            {
                errorCounter++;
                subscriber.OnError(exception);
                if (!storageManager.IsActive()) storageManager = StorageManager.GetStorageManager();
            }catch(Exception ex)
            {
                Logger.Error(storageManager,ex);
            }            
        }

        private void MonitorQueue(IAsyncCursor<BusMessage<t>> cursor, StorageManager storageManager)
        {
            using (cursor)
            {
                while (cursor.MoveNext())
                {
                    FireEvent(cursor, storageManager);
                }
            }                
        }

        private void FireEvent(IAsyncCursor<BusMessage<t>> cursor, StorageManager storageManager)
        {
            List<BusMessage<t>> messages = cursor.Current.ToList();
            FilterMessagesForTTL(messages);
            BusMessage<t> lastMessage = messages.LastOrDefault();

            if (lastMessage.IsNotNull())
            {

                ProcessedMessageTrack track = DBUtil.ToMessageTrack(lastMessage._id, lastMessage.CreationTime, lastMessage.Message.GetType(), this.listenerApplication);
                storageManager.SaveProcessedMessageTrack(track);
                FireEvent(messages.Select(x => x.Message).ToList());
            }
        }

        private void FilterMessagesForTTL(List<BusMessage<t>> messages)
        {
            foreach(var message in messages)
            {
                if(message.TTL.IsNotNull() && message.TTL < DateTime.Now)
                {
                    messages.Remove(message);
                }
            }            
        }

        private void FireEvent(List<t> messages)
        {
            if (messages.Count > 1)
            {
                subscriber.OnEvents(messages);
            }
            else if (messages.Count > 0)
            {
                subscriber.OnEvent(messages.ElementAt(0));
            }
        }

        private FilterDefinition<BusMessage<t>> GetMessagesFilter(StorageManager storageManager)
        {
            messageTrack = storageManager.GetTrackFor(this.listenerApplication);
            

            FilterDefinition<BusMessage<t>> filter = Builders<BusMessage<t>>.Filter.Where(x => x.SenderApplicationName != listenerApplication.ApplicationName); 
            var lastTrackFilter = Builders<BusMessage<t>>.Filter.Where(x => x._id > messageTrack.EventID);
            var sourceFilter = Builders<BusMessage<t>>.Filter.Where(x=> subscriber.InterestedInSources.Contains(x.SenderApplicationName));
            
            if (messageTrack.IsNotNull())
            {
                filter = Builders<BusMessage<t>>.Filter.And(new FilterDefinition<BusMessage<t>>[]{filter,lastTrackFilter });
            }

            if(subscriber.InterestedInSources.IsNotNull() && subscriber.InterestedInSources.Count > 0)
            {
                filter = Builders<BusMessage<t>>.Filter.And(new FilterDefinition<BusMessage<t>>[] { filter, sourceFilter });
            }
            return filter;
        }
    }
}
