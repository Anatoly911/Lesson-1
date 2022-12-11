using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Messaging
{
    public class Producer
    {
        private readonly string _queueName;
        private readonly string _hostName;

        public Producer(string queueName, string hostName)
        {
            _queueName = queueName;
            _hostName = "rattlesnake.rmq.cloudamqp.com";
        }
        public void Send(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5672,
                UserName = "fxcrpbaz",
                Password = "N9rDDvDKOFuyzHbnfXV9GH5s-XB_j_T8",
                VirtualHost = "fxcrpbaz"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare("direct_exchange", "direct", false, false, null);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "direct_exchange", routingKey: _queueName, basicProperties: null, body: body);
        }
    }
}
