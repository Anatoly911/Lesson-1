using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Messages
{
    public interface IBookingRequest
    {
        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public string CreationDate { get; }
        Dish? PreOrder { get; }
    }
    public class BookingRequest : IBookingRequest
    {
        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public string CreationDate { get; }
        public Dish? PreOrder { get; }
        public BookingRequest(Guid orderId, Guid clientId, Dish? preOrder, string creationDate)
        {
            OrderId = orderId;
            ClientId = clientId;
            CreationDate = creationDate;
            PreOrder = preOrder;
        }
    }
}
