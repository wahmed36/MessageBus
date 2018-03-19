using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MessageBus.Config
{
    public class SupportedMessageTypeCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override ConfigurationElement CreateNewElement()
        {
            return new SupportedType();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            SupportedType supportedType = element as SupportedType;
            return supportedType.Value;
        }

        protected override string ElementName
        {
            get
            {
                return ConfigLabels.SUPPORTED_TYPE;
            }
        }

        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrEmpty(elementName) && elementName.Equals(ConfigLabels.SUPPORTED_TYPE);
        }

        public SupportedType this[int index]
        {
            get
            {
                return base.BaseGet(index) as SupportedType;
            }
        }

        public new SupportedType this[string key]
        {
            get
            {
                return base.BaseGet(key) as SupportedType;
            }
            set
            {
                base.BaseAdd(value);
            }
        }
    }

}
