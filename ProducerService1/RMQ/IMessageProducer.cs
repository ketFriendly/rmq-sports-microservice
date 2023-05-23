using ProducerService1.DTOs;

namespace ProducerService1.RMQ
{
    public interface IMessageProducer
    {
        void SendMessage(int match_id, MessageDTO message);
    }
}
