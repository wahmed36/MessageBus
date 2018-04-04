using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBus.Config;
using MessageBus.DataBase.Mongo;
using MongoDB.Bson;
using System.Security.Cryptography;

namespace MessageBus.DataBase
{
    public static class DBUtil
    {
        public static string GetCollectionName(Type type)
        {
            if(type.IsNull())
            {
                throw new ArgumentNullException();
            }

            int LAST_NUMBERS = 5;

            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(type.FullName));
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var number in hash)
                {
                    stringBuilder.Append(number.ToString("X2"));
                }

                string hexString = stringBuilder.ToString();
                int code = int.Parse(hexString.Substring(hexString.Length - LAST_NUMBERS), System.Globalization.NumberStyles.HexNumber);
                
                return string.Concat(type.Name, "_", code);
            }           
        }

        public static ProcessedMessageTrack ToMessageTrack(ObjectId eventID, DateTime eventTime, Type messageType, ApplicationInfo application)
        {
            if(eventID.IsNull() || eventTime.IsNull() || messageType.IsNull() || application.IsNull())
            {
                throw new ArgumentNullException();
            }

            return new ProcessedMessageTrack
            {
                EventID = eventID,
                EventTime = eventTime,
                MessageType = messageType.FullName,
                ProcessingApplication = application
            };
        }
    }
}
