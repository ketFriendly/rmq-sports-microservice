using System.ComponentModel.DataAnnotations;

namespace ProducerService1.DTOs
{
    public class RequestDTO
    {
        public RequestDTO()
        {
            match_ids = new List<int>();
        }
        [Required]
        public List<int> match_ids { get; set; }
        
        public List<int>? market_ids { get; set; }
    }
}
