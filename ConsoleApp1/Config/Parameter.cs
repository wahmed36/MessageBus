using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MessageBus.Config
{
    public class Parameter:ConfigurationElement
    {
        private const string PARAMETER_NAME = "name";
        private const string PARAMETER_VALUE = "value";

        [ConfigurationProperty(PARAMETER_NAME, IsRequired =true, IsKey =true)]        
        public string Name
        {
            get
            {
                return this[PARAMETER_NAME] as string;
            }
            set
            {
                this[PARAMETER_NAME] = value;
            }
        }

        [ConfigurationProperty(PARAMETER_VALUE, IsRequired =true)]
        public string Value
        {
            get
            {
                return this[PARAMETER_VALUE] as string;
            }
            set
            {
                this[PARAMETER_VALUE] = value;
            }
        }
    }
}
