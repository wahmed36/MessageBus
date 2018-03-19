using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MessageBus.Config
{
    public static class Constants
    {
        //database parameter keys
        public const string MONGODB_CONNECTION_NAME = "MONGO_CONNECTION";
        public const string MONGODB_NAME = "MONGODB_NAME";
        public const string PROCESSED_MESSAGES_COLLECTION = "PROCESSING_TRACK";

        //General Parameter keys
        public const string Max_NUMBER_OF_DOCUMENTS = "MaxDocuments";
        public const string MAX_COLLECTION_SIZE_INBYTES = "MaxSize";

        //Logger constants
        public const string LOG_FILE_NAME = "MessageBus";
    }
}
