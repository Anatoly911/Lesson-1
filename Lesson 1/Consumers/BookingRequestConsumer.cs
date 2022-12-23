using MassTransit;
using Microsoft.Extensions.Logging;
using Restaurant.Booking;
using Restaurant.Messages;
using Restaurant.Messages.InMemoryDb;
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
        private readonly IInMemoryRepository<BookingRequestModel> _repository;
        private ILogger _logger;
        public RestaurantBookingRequestConsumer(Restaurant restaurant, IInMemoryRepository<BookingRequestModel> repository, ILogger<RestaurantBookingRequestConsumer> logger)
        {
            _restaurant = restaurant;
            _repository = repository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId}]");
            var model = _repository.Get().FirstOrDefault(i => i.OrderId == context.Message.OrderId);
            if (model is not null && model.CheckMessageId(context.MessageId.ToString()))
            {
                Console.WriteLine(context.MessageId.ToString());
                Console.WriteLine("SecondTime");    
                return;
            }
            var requestModel = new BookingRequestModel(context.Message.OrderId, context.Message.ClientId, 
                context.Message.PreOrder, DateTime.Parse(context.Message.CreationDate), context.MessageId.ToString());
            Console.WriteLine(context.MessageId.ToString());
            Console.WriteLine("First time");
            var resultModel = model?.Update(requestModel, context.MessageId.ToString()) ?? requestModel;
            _repository.AddOrUpdate(resultModel);
            var result = await _restaurant.BookFreeTableAsync(1);
            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, result ?? false));
        }
    }
}
