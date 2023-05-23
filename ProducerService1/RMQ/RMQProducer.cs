using Newtonsoft.Json;
using ProducerService1.DTOs;
using RabbitMQ.Client;
using System.Text;

namespace ProducerService1.RMQ
{
    public class RabbitMQProducer : IMessageProducer
    {
        private readonly IModel _channel;

        public RabbitMQProducer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.ExchangeDeclare("my_headers_exchange", ExchangeType.Headers, durable: true);
            _channel.QueueDeclare("shared_queue", true, false, false, null);
        }

        public void SendMessage(int match_id, MessageDTO message)
        {
            string headerValue = match_id.ToString();
            var properties = _channel.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object>
            {
                { "header_key", headerValue }
            };
            properties.Persistent = true;
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish("my_headers_exchange", "shared_queue", properties, body);
            Console.WriteLine("Message with header value sent.");
        }
    }
    /*    public class RabbitMQProducer : IMessageProducer
        {
            public void SendMessage<T>(T message)
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost"
                };

                var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare("matches", exclusive: false);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: "matches", body: body);
            }
        }*/
}
