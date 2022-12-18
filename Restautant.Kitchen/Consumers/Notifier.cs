using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Kitchen.Consumers
{
    public class Notifier
    {
        public void Notify(Guid clientId, Guid orderId, string message) 
        {
            Console.WriteLine($"OrderId {orderId}: Уважаемый клиент {clientId}! {message}");
        }
    }
}
