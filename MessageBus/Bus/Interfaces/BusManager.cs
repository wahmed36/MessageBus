using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBus.Message;
namespace MessageBus.Interfaces
{
    /// <summary>
    /// Main interface to define publisher subscriber model
    /// </summary>
    /// <typeparam name="t"></typeparam>
    public interface BusManager<t>
    {        
        Response SendMessage(t message, ApplicationInfo senderApplication, TimeSpan messageTTL = default(TimeSpan));
        void Subscribe(Subscriber<t> subscriber, ApplicationInfo listenerApplication);
    }
}
