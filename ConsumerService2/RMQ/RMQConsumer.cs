using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace ConsumerService2.RMQ
{
    public class RabbitMQConsumerService : IHostedService, IDisposable
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;

        public RabbitMQConsumerService()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("matches", exclusive: false);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message received: {message}");
            };

            _channel.BasicConsume(queue: "matches", autoAck: true, consumer: _consumer);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _channel.BasicConsume(queue: "matches", autoAck: true, consumer: _consumer);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();
            return Task.CompletedTask;
        }
    }

}
