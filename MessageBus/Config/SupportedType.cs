using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MessageBus.Config
{
    public class SupportedType : ConfigurationElement
    {
        private const string AttributeName = "value";
        //private ParamsCollection paramsCollection = new ParamsCollection();

        [ConfigurationProperty(AttributeName, IsRequired = true)]
        [StringValidator(InvalidCharacters = "? |@#%^&*()")]
        public string Value
        {
            get
            {
                return (string)this[AttributeName];
            }
            set
            {
                this[AttributeName] = value;
            }
        }

        [ConfigurationProperty("", IsRequired =false, IsDefaultCollection =true)]
        [ConfigurationCollection(typeof(ParamsCollection),AddItemName =ConfigLabels.PARAMETER)]
        public ParamsCollection Params
        {
            get
            {
                return this[""] as ParamsCollection;
            }
            set
            {
                this[""] = value;
            }
        }
    }
}
