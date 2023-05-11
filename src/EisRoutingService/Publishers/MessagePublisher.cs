using EisRoutingService.Connections;
using EisRoutingService.Messages;
using System.Text.Json;
using ActiveMQ.Artemis.Client;



namespace EisRoutingService.Publishers;

public class MessagePublisher : IMessagePublisher
{
    private readonly IConnection _channel;

    public MessagePublisher(IChannelFactory channelFactory)
        => _channel = channelFactory.Create();

    public async Task PublishAsync<TMessage>(string exchange, string routingKey, TMessage message, string messageId = default)
        where TMessage : class, IMessage
    {
        var json = JsonSerializer.Serialize(message);
        await using var producer = await _channel.CreateProducerAsync(exchange, RoutingType.Anycast);

        var msg = new Message(json);
        await producer.SendAsync(msg);

        await Task.CompletedTask;
    }
}
