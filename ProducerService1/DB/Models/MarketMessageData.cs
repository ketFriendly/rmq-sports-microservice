using ProducerService1.Interfaces;

namespace ProducerService1.DB.Models
{
    public class MarketMessageData : IMessageData
    {
        public int Id { get; set; }
        public int matchId { get; set; }
        public int marketId { get; set; }
        public decimal value { get; set; }
        public string name { get; set; }
        public string sport { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime startTime { get; set; }
       
    }

}
