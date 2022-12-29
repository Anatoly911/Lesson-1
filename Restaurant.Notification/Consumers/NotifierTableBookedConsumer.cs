using MassTransit;
using Microsoft.Extensions.Logging;
using Restaurant.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Notification.Consumers
{
    public class NotifierTableBookedConsumer : IConsumer<ITableBooked>
    {
        private ILogger _logger;
        private readonly Notifier _notifier;
        public NotifierTableBookedConsumer(ILogger<NotifierTableBookedConsumer> logger, Notifier notifier)
        {
            _notifier = notifier;
            _logger = logger;
        }
        public Task Consume(ConsumeContext<ITableBooked> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId}]");
            var result = context.Message.Success;
            _notifier.Accept(context.Message.OrderId, result ? Accepted.Booking : Accepted.Rejected, context.Message.ClientId);
            return Task.CompletedTask;
        }
    }
}
