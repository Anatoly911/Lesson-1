using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    public class Consumer: IDisposable
    {
        private readonly string _queueName;
        private readonly string _hostName;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public Consumer (string queueName, string hostName)
        {
            _queueName = queueName;
            _hostName = "rattlesnake.rmq.cloudamqp.com";
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5672,
                UserName = "fxcrpbaz",
                Password = "N9rDDvDKOFuyzHbnfXV9GH5s-XB_j_T8",
                VirtualHost = "fxcrpbaz"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallBack)
        {
            _channel.ExchangeDeclare(exchange: "direct_exchange", type: "direct");
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: _queueName, exchange: "direct_exchange", routingKey: _queueName);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += receiveCallBack;
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }
        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}
