using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_1.Consumers
{
    public interface IBookingExpire
    {
        public Guid OrderId { get; }
    }
    public class BookingExpire : IBookingExpire
    {
        private readonly RestaurantBooking _instance;
        public BookingExpire(RestaurantBooking instance)
        {
            _instance = instance;
        }
        public Guid OrderId => _instance.OrderId;
    }
}
