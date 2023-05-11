using EisRoutingService.Contracts;
using EisRoutingService.Publishers;
using EisRoutingService.Services;
using System.Diagnostics.Contracts;

namespace EisRoutingService.Messages.Handlers;

public class BookMessageHandler : IMessageHandler<BookCreated>
{
    private readonly ILogger<MessagingBackgroundService> _logger;
    private readonly IMessagePublisher _publisher;
    public BookMessageHandler(ILogger<MessagingBackgroundService> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }


    public async Task HandleAsync(BookCreated message)
    {
        _logger.LogInformation(
                      $"Handler message : {message.Id} | Funds: {message.Title} | Author : {message.Author}");
        await _publisher.PublishAsync("queue.inbound.book", "queue.inbound.book", message);

    }
}