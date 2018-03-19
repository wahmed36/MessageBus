using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MessageBus
{
    public static class Extentions
    {
        public static bool IsNull(this Object obj)
        {
            if(obj == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNotNull(this Object obj)
        {
            if(IsNull(obj))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string ToJSON(this object message)
        {
            if (message.IsNull()) return string.Empty;

            return JsonConvert.SerializeObject(message);
        }
    }
}
