using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;
using System.Xml;


namespace MessageBus.Config
{   
    public class MessageBusSection: ConfigurationSection
    {
        public MessageBusSection()
        {            
        }

        [ConfigurationProperty(ConfigLabels.SUPPORTED_MESSAGE_TYPE, IsDefaultCollection =true)]
        public SupportedMessageTypeCollection SupportedMessages
        {
            get
            {
                return this[ConfigLabels.SUPPORTED_MESSAGE_TYPE] as SupportedMessageTypeCollection;
            }
            set
            {
                this[ConfigLabels.SUPPORTED_MESSAGE_TYPE] = value;
            }
        }
        
        [ConfigurationProperty(ConfigLabels.DEFAULT_PARAMETERS, IsRequired =false)]
        public ParamsCollection Parameters
        {
            get
            {
                return this[ConfigLabels.DEFAULT_PARAMETERS] as ParamsCollection;
            }

            set
            {
                this[ConfigLabels.DEFAULT_PARAMETERS] = value;
            }
        }
    }
      
}
