using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBus.Interfaces;
using MessageBus.Impl;

namespace MessageBus
{
    /// <summary>
    /// Generic facotry to provide instances of all public interfaces exposed by Message bus
    /// </summary>
    public static class ObjectFactory
    {   
        public static BusManager<t> GetBusManager<t>()
        {
            return BusManagerImpl<t>.GetInstance();
        }
    }
}
