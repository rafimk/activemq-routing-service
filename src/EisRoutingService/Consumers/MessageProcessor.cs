using System.ComponentModel;
using System.Diagnostics;

namespace EisRoutingService.Consumers
{
    public class MessageProcessor
    {
        public void Process(string message)
        {
            Console.WriteLine("Message : ", message);
        }
    }
}
