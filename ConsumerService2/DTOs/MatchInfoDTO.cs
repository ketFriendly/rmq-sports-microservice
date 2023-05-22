using ConsumerService2.Utils;

namespace ConsumerService2.DTOs
{
    public class MatchInfoDTO
    {
        public int match_id { get; set; }
        public int market_id { get; set; }
        public decimal value { get; set; }
        public EType type { get; set; }
        public string sport { get; set; }
        public string name { get; set; }
        public DateTime start_time { get; set; }
    }

}
