using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProducerService1.DTOs;
using ProducerService1.RMQ;
using ProducerService1.Services;

namespace RMQSportsMicroservices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMessageProducer _messagePublisher;
        private readonly IMatchService _service;
        public MatchesController(IMessageProducer messagePublisher, IMatchService service)
        {
            _messagePublisher = messagePublisher;
            _service = service;
        }

        [HttpPost]
        [EnableCors]
        public async Task<IActionResult> CreateMessage(RequestDTO request)
        {
            var dtos = await _service.getData(request.match_ids, request.market_ids);
            foreach (var message in dtos)
            {
                _messagePublisher.SendMessage(message);
            }
            _messagePublisher.SendMessage("stop");
            return Ok(dtos);
        }
       
    }
}