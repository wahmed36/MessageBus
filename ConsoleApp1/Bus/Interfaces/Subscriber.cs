using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBus.Message;

namespace MessageBus.Interfaces
{
    public interface Subscriber<t> 
    {
        //List of application Names as defined by each publisher in its Application info's name property. 
        //If defined then subscriber will receive message only from those defined sources
        List<string> InterestedInSources { get; set; }
        void OnEvent(t message);
        void OnEvents(List<t> messages);
        void OnError(Exception exception);
    }
}
