using System.Text;

namespace ConsumerService2.Services
{
    public interface IMessageConsumer
    {
        void ConsumeMessage(byte[] body);
    }
}
