using EisRoutingService.Contracts;
using EisRoutingService.Messages;
using EisRoutingService.Publishers;
using EisRoutingService.Subscribers;
using Microsoft.AspNetCore.Mvc;

namespace EisRoutingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IMessagePublisher _publisher;
        private readonly IMessageSubscriber _subscriber;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IMessagePublisher publisher,
            ILogger<WeatherForecastController> logger,
            IMessageSubscriber subscriber)
        {
            _publisher = publisher;
            _logger = logger;
            _subscriber = subscriber;
        }

        [HttpGet("Publish")]
        public async Task<ActionResult<BookCreated>> Publish()
        {
            var contract = new BookCreated
            {
                Id = Guid.NewGuid(),
                Title = "Test Title",
                Author = "Muhammed Rafi",
                Cost = 10,
                InventoryAmount = 20,
                UserId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow
            };

            await _publisher.PublishAsync("queue.inbound.test", "queue.inbound.test", contract);

           return Ok(contract);
        }

        [HttpGet("Subscribe")]
        public async Task Subscribe()
        {
            await _subscriber
            .SubscribeMessage<BookCreated>("topic.outbound.book", "topic.outbound.book", "Funds",
                async (msg) =>
                {
                    _logger.LogInformation(
                        $"Received message : {msg.Id} | Funds: {msg.Title} | Author : {msg.Author}");
                });


        }

    }
}