using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MessageBus.Config
{
    public class ParamsCollection : ConfigurationElementCollection
    {
        protected override string ElementName => ConfigLabels.PARAMETER;
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrEmpty(elementName) && ConfigLabels.PARAMETER.Equals(elementName);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Parameter() as ConfigurationElement;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as Parameter).Name;
        }

        public Parameter this[int index]
        {
            get
            {
                return base.BaseGet(index) as Parameter;
            }
        }

        public new Parameter this[string key]
        {
            get
            {
                return base.BaseGet(key) as Parameter;
            }
            set
            {
                base.BaseAdd(value);
            }
        }
    }
}
