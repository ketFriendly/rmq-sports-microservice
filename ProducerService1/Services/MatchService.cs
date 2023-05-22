using Microsoft.EntityFrameworkCore;
using ProducerService1.DB;
using ProducerService1.DB.Models;
using ProducerService1.DTOs;
using ProducerService1.Interfaces;
using ProducerService1.Utils;

namespace ProducerService1.Services
{
    public class MatchService: IMatchService
    {
        public readonly TestDbContext db;
        private readonly IMatchesListSingleton _matchesListSingleton;
        public MatchService(TestDbContext db, IMatchesListSingleton matchesListSingleton)
        {
            this.db = db;
            _matchesListSingleton = matchesListSingleton;
        }

        public async Task<IEnumerable<MessageDTO>> getData(List<int> match_ids, List<int> ?market_ids)
        {
            var list = (market_ids == null || market_ids.Count == 0)  switch
            {
                true => await db.Set<MarketMessageData>()
                        .FromSqlRaw($"SELECT market.id, matchId, marketId, value, name, sport, timestamp, startTime\r\nFROM market \r\n\tINNER JOIN match ON market.matchId = match.id\r\n\tLEFT JOIN sport ON match.sportId = sport.id\r\nWHERE \r\n\tmatchId IN ({string.Join(",", match_ids)}) \r\nORDER BY timestamp")
                        .ToListAsync(),
                false => await db.Set<MarketMessageData>()
                        .FromSqlRaw($"SELECT market.id, matchId, marketId, value, name, sport, timestamp, startTime\r\nFROM market \r\n\tINNER JOIN match ON market.matchId = match.id\r\n\tLEFT JOIN sport ON match.sportId = sport.id\r\nWHERE \r\n\tmatchId IN ({string.Join(",", match_ids)}) \r\n\tAND marketId IN ({string.Join(",", market_ids)})\r\nORDER BY timestamp")
                        .ToListAsync()
            };
            List<MessageDTO> dtos = new List<MessageDTO>();
            foreach (IMessageData message in list)
            {
                var dto = MessageToDto(message);
                dtos.Add(dto);
            }
            return dtos;
        }

        private MessageDTO MessageToDto(IMessageData msg)
        {
            bool containsMatch = _matchesListSingleton.Contains(msg.matchId);
            MessageDTO dto = new MessageDTO()
            { 
                match_id = msg.matchId,
                market_id = msg.marketId,
                value = msg.value,
                type = containsMatch ? EType.MatchChange : EType.MatchCreate, 
                sport = msg.sport,
                name = msg.name,
                start_time = msg.startTime
            };
            if (!containsMatch)
            {
                _matchesListSingleton.AddMatch(msg.matchId);
            }
             
            return dto;
        }
    }
}
