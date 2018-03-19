using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;
using MessageBus.Log;
/// <summary>
/// Class to abstract configuration file
/// </summary>
namespace MessageBus.Config
{
    public static class ConfigManager
    {
        /// <summary>
        /// Finds out connection string for the mongoDB
        /// </summary>
        /// <returns></returns>
        public static string GetMongoDBConnection()
        {
            return GetConnectionString(Constants.MONGODB_CONNECTION_NAME);             
        }

        /// <summary>
        /// Returns connection string with the given name. If connection string is not directly found
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionName)
        {
            string result = string.Empty;
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[connectionName];
            
            if(connectionString == null)
            {
                string connName = GetAppSetting(connectionName);
                connectionString = ConfigurationManager.ConnectionStrings[connName];
            }

            if (connectionString != null)
            {
                result = connectionString.ConnectionString;
            }

            return result;
        }

        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static List<SupportedType> LoadSupportedTypes()
        {
            MessageBusSection busSection = (MessageBusSection)ConfigurationManager.GetSection(ConfigLabels.MESSAGE_BUS_SECTION);
            SupportedMessageTypeCollection collection = busSection.SupportedMessages;
            IEnumerator enumerator = collection.GetEnumerator();
            List<SupportedType> result = new List<SupportedType>();

            while (enumerator.MoveNext())
            {
                SupportedType current = enumerator.Current as SupportedType;
                result.Add(current);
            }
            return result;
        }

        public static String GetStringParameter(string parameterName)
        {
            Parameter parameter = GetParameter(parameterName);
            
            if(parameter.IsNotNull())
            {
                return parameter.Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public static int GetIntParameter(string parameterName, int defaultValue = 0)
        {
            Parameter param = GetParameter(parameterName);
            return ConvertParamToInt(param, defaultValue);
        }

        private static int ConvertParamToInt(Parameter param, int defaultValue)
        {
            int result = defaultValue;
            if (param.IsNotNull() && int.TryParse(param.Value, out result)) { };
            return result;
        }

        public static int GetIntParameter(String type, string parameterName, int defaultValue = 0)
        {
            SupportedType supportedType = LoadSupportedTypes().Where(x => x.Value == type).SingleOrDefault();
            if (supportedType.IsNotNull() && supportedType.Params.IsNotNull() )
            {
                IEnumerator paramsEnumerator = supportedType.Params.GetEnumerator();
                while (paramsEnumerator.MoveNext())
                {
                    Parameter parameter = paramsEnumerator.Current as Parameter;
                    if (parameter.Name.Equals(parameterName))
                    {
                        return ConvertParamToInt(parameter, defaultValue);
                    }
                }
            }
            return GetIntParameter(parameterName, defaultValue);
        }

        public static bool GetBooleanParameter(string parameterName, bool defaultValue = false)
        {
            Parameter param = GetParameter(parameterName);
            bool result = defaultValue;

            if (param.IsNotNull() && bool.TryParse(param.Value, out result)) { };
            
            return result;
        }

        private static Parameter GetParameter(string parameterName)
        {
            return GetAllParameters().Where(x => x.Name.Equals(parameterName)).SingleOrDefault();
        }
        public static List<Parameter> GetAllParameters()
        {
            MessageBusSection busSection = (MessageBusSection)ConfigurationManager.GetSection(ConfigLabels.MESSAGE_BUS_SECTION);
            IEnumerator enumerator = busSection.Parameters.GetEnumerator();
            List<Parameter> result = new List<Parameter>();

            while (enumerator.MoveNext())
            {
                Parameter param = enumerator.Current as Parameter;
                result.Add(param);
            }

            return result;
        }
    }
}
