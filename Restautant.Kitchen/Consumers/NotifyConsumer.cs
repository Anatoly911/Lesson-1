using MassTransit;
using Restaurant.Kitchen.Consumers;
using Restaurant.Messages;
using Restaurant.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Notification.Consumers
{
    public class NotifyConsumer : IConsumer<INotify>
    {
        private readonly Notifier _notifier;
        public NotifyConsumer(Notifier notifier)
        {
            _notifier = notifier;
        }
        public Task Consume(ConsumeContext<INotify> context)
        {
            _notifier.Notify(context.Message.OrderId, context.Message.ClientId, context.Message.Message);
            return Task.CompletedTask;
        }
        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            var rnd = new Random().Next(1000, 10000);
            if (rnd > 8000)
                throw new Exception("Случилась какая-то беда!");
            Console.WriteLine($"[OrderId: {context.Message.OrderId}] Проверка на кухне займет: {rnd}");
            await Task.Delay(rnd);
        }
    }
}
