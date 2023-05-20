namespace ProducerService1.RMQ
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}
