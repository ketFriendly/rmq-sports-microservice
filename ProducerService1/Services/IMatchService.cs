using ProducerService1.DTOs;

namespace ProducerService1.Services
{
    public interface IMatchService
    {
        public Task<IEnumerable<MessageDTO>> getData(List<int> match_ids, List<int> ?market_ids);
    }
}
