using ProducerService1.Utils;

namespace ProducerService1.DTOs
{
    public class MessageDTO
    {
        public int match_id { get; set; }
        public int market_id { get; set; }
        public decimal value { get; set; }
        public EType type { get; set; }
        public required string sport { get; set; }
        public required string name { get; set; }
    }
}
