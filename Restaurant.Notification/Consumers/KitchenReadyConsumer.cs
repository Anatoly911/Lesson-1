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
    public class KitchenReadyConsumer : IConsumer<IKitchenReady>
    {
        private ILogger _logger;
        private readonly Notifier _notifier;
        public KitchenReadyConsumer(ILogger<KitchenReadyConsumer> logger, Notifier notifier)
        {
            _notifier = notifier;
            _logger = logger;
        }
        public Task Consume(ConsumeContext<IKitchenReady> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId}]");
            _notifier.Accept(context.Message.OrderId, Accepted.Kitchen);
            return Task.CompletedTask;
        }
    }
}
