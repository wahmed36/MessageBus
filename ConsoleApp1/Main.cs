using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBus.Config;

namespace MessageBus
{
    class Program
    {
        static void Main(string[] args)
        {
            string result= ConfigManager.GetMongoDBConnection();

            Console.Write(result);
        }
    }
}
