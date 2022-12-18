using MassTransit;
using Restaurant.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_1.Consumers
{
    public class RestaurantBookingRequestConsumer : IConsumer<IBookingRequest>
    {
        private readonly Restaurant _restaurant;
        public RestaurantBookingRequestConsumer(Restaurant restaurant)
        {
            _restaurant = restaurant;
        }
        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            Console.WriteLine($"[OrderId: {context.Message.OrderId}]");
            var result = await _restaurant.BookFreeTableAsync(1);
            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, context.Message.ClientId, result ?? false));
        }
    }
}
