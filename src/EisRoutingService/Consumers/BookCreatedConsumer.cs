using EisRoutingService.Client;
using EisRoutingService.Contracts;

namespace EisRoutingService.Consumers;

public class BookCreatedConsumer : ITypedConsumer<BookCreated>
{
    private readonly ILogger<BookCreatedConsumer> _logger;

    public BookCreatedConsumer(ILogger<BookCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task ConsumeAsync(BookCreated message, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Message received : {message.Title}");
        Console.WriteLine($" << {message?.ToString()} >>");
        await Task.CompletedTask;
    }
}