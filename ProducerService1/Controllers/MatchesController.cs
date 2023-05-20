using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProducerService1.RMQ;

namespace RMQSportsMicroservices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMessageProducer _messagePublisher;
        public MatchesController(IMessageProducer messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        [HttpPost]
        [EnableCors]
        public async Task<IActionResult> CreateMessage(String message)
        {
            _messagePublisher.SendMessage(message);
            return Ok(message);
        }
       
    }
}