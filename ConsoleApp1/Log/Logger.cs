using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.Reflection;
using MessageBus.Config;

/// <summary>
/// This class will act as a facade of Log4net framework. 
/// </summary>
namespace MessageBus.Log
{
    public class Logger
    {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static Logger()
        {
            XmlConfigurator.Configure() ;
            //Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public static void Debug(object message, Exception ex = null)
        {
            if(ex.IsNotNull() && message.IsNotNull())
            {
                Log.Debug(message,ex);
            }else if(message.IsNotNull())
            {
                Log.Debug(message);
            }            
        }

        public static void Fatal(object message, Exception ex = null)
        {
            if(message.IsNotNull() && ex.IsNotNull())
            {
                Log.Fatal(message,ex);
            }
            else if(message.IsNotNull())
            {
                Log.Fatal(message);
            }            
        }

        public static void Error(object message, Exception ex)
        {
            if (message.IsNotNull() && ex.IsNotNull())
            {
                Log.Error(message, ex);
            }
            else if (message.IsNotNull())
            {
                Log.Error(message);
            }
        }

        public static void Warn(object message, Exception ex)
        {
            if (message.IsNotNull() && ex.IsNotNull())
            {
                Log.Warn(message, ex);
            }
            else if (message.IsNotNull())
            {
                Log.Warn(message);
            }
        }

        public static void Info(object message, Exception ex)
        {
            if (message.IsNotNull() && ex.IsNotNull())
            {
                Log.Info(message, ex);
            }
            else if (message.IsNotNull())
            {
                Log.Info(message);
            }
        }
    }
}
