using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Messages
{
    public interface INotify
    {
        public string Message { get; set; }
        public Guid OrderId { get; }
        public Guid ClientId { get; }
       
    }
    public class Notify : INotify
    {
        public Notify(Guid orderId, Guid clientId, string message)
        {
            OrderId = orderId;
            ClientId = clientId;
        }
        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public string Message { get; set; }
    }
}
