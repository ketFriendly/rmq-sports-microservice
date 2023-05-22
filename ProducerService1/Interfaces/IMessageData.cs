namespace ProducerService1.Interfaces
{
    public interface IMessageData
    {
        int Id { get; }
        int matchId { get; }
        int marketId { get; }
        decimal value { get; }
        string sport { get; }
        string name { get; }
        DateTime timestamp { get; }
        DateTime startTime { get; }

    }
}
