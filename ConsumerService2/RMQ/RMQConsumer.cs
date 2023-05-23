/*using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using ConsumerService2.Services;
using Microsoft.Extensions.Hosting;

namespace ConsumerService2.RMQ
{
    public class RabbitMQConsumerService : IHostedService, IDisposable
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;
        private readonly IMessageConsumer _messageConsumer;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private string _consumerTag;

        public RabbitMQConsumerService(IMessageConsumer messageConsumer, IHostApplicationLifetime applicationLifetime)
        {
            _messageConsumer = messageConsumer;
            _applicationLifetime = applicationLifetime;
            _consumerTag = Guid.NewGuid().ToString();
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
                if (message == "\"stop\"")
                {
                    _channel?.BasicCancel(consumerTag: _consumerTag);
                    _applicationLifetime.StopApplication();
                }
                else
                {
                    _messageConsumer.ConsumeMessage(body);
                }
            };
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumerTag = _channel.BasicConsume(queue: "matches", autoAck: true, consumer: _consumer, consumerTag: _consumerTag);
            
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();
            await Task.Delay(500);
        }
    }
}*/

using ConsumerService2.Services;
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
        private readonly IMessageConsumer _messageConsumer;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private string _consumerTag;

        public RabbitMQConsumerService(IMessageConsumer messageConsumer, IHostApplicationLifetime applicationLifetime)
        {
            _messageConsumer = messageConsumer;
            _applicationLifetime = applicationLifetime;
            _consumerTag = Guid.NewGuid().ToString();
            _factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("my_headers_exchange", ExchangeType.Headers, durable:true);
            _channel.QueueDeclare("shared_queue", true, false, false, null);

            _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += (model, ea) =>
            {
                var headerValue = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["header_key"]);

                var exclusiveQueueName = _channel.QueueDeclare().QueueName;

                _channel.BasicPublish("my_headers_exchange", exclusiveQueueName, ea.BasicProperties, ea.Body);

                var headers = new Dictionary<string, object>
                {
                { "header_key", headerValue },
                { "x-match", "all" }
                };

                _channel.QueueBind(exclusiveQueueName, "my_headers_exchange", string.Empty, headers);

                var exclusiveQueueConsumer = new EventingBasicConsumer(_channel);
                exclusiveQueueConsumer.Received += (model, e) =>
                {
                    var body = e.Body.ToArray();
                    _messageConsumer.ConsumeMessage(body);
                };

                _channel.BasicConsume(exclusiveQueueName, true, exclusiveQueueConsumer);
            };
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            _consumerTag = _channel.BasicConsume(queue: "shared_queue", autoAck: true, consumer: _consumer, consumerTag: _consumerTag);
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();
            await Task.Delay(500);
        }
    }
}