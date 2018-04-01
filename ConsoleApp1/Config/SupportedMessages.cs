using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MessageBus.Message;
using System.Reflection;

/// <summary>
/// Singleton class that contains a list of all properly configured supported Messages types.
/// </summary>
namespace MessageBus.Config
{    
    public class SupportedMessages : List<Type>
    {
        //for singleton instance
        private static SupportedMessages mapping = new SupportedMessages();

        protected SupportedMessages()
        {
            List<SupportedType> types = ConfigManager.LoadSupportedTypes();
            foreach(var typ in types)
            {
                AddMapping(CreateType(typ.Value));
            }
        }

        protected Type CreateType(string type)
        {
            Type result = Type.GetType(type);
            if (result != null) return result;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                result = assembly.GetType(type);
                if (result != null)
                {
                    object instance = Activator.CreateInstance(result);
                    return result;
                }                    
            }
            throw new ArgumentException(string.Format("{0} type not found in any any assembly",type));
            
        }

        public static SupportedMessages GetInstance()
        {
            return mapping;
        }

        public void AddMapping(Type type)
        {
            if(type.IsNotNull() && !this.Contains(type))
            {
                this.Add(type);
            }            
        }
        
        public bool IsTypeSupported(Type type)
        {
            if(this.Contains(type))
            {
                return  true;                
            }
            else
            {
                return false;
            }            
        }
    }
}
