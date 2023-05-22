using System.Text;

namespace ConsumerService2.Services
{
    public class MessageConsumerService : IMessageConsumer
    {
        public void ConsumeMessage(byte[] body)
        {
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Message received: {message}");
        }
    }
}
