using EisRoutingService.Contracts;
using EisRoutingService.Messages;
using EisRoutingService.Subscribers;

namespace EisRoutingService.Services;

public class MessagingBackgroundService : BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly IMessageDispatcher _dispatcher;
    private readonly ILogger<MessagingBackgroundService> _logger;

    public MessagingBackgroundService(IMessageSubscriber messageSubscriber, IMessageDispatcher dispatcher,
        ILogger<MessagingBackgroundService> logger)
    {
        _messageSubscriber = messageSubscriber;
        _dispatcher = dispatcher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       await _messageSubscriber
            .SubscribeMessage<BookCreated>("topic.outbound.book", "topic.outbound.book", "Funds",
                async (msg) =>
                {
                    _logger.LogInformation(
                        $"Received message : {msg.Id} | Funds: {msg.Title} | Author : {msg.Author}");
                    await _dispatcher.DispatchAsync(msg);
                });
            
    }
}