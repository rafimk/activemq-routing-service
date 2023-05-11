using ActiveMQ.Artemis.Client;
using EisRoutingService.Connections;
using EisRoutingService.Messages;
using System.Text.Json;

namespace EisRoutingService.Subscribers;

public class MessageSubscriber : IMessageSubscriber
{
    private readonly IConnection _channel;

    public MessageSubscriber(IChannelFactory channelFactory) => _channel = channelFactory.Create();

    public async Task<IMessageSubscriber> SubscribeMessage<TMessage>(string queue, string routingKey, string exchange,
        Func<TMessage, Task> handle) where TMessage : class, IMessage
    {

        await using var consumer = await _channel.CreateConsumerAsync(queue, RoutingType.Anycast);

        var msg = await consumer.ReceiveAsync();
        await consumer.AcceptAsync(msg);

        var body = msg.GetBody<string>();
        var message = JsonSerializer.Deserialize<TMessage>(body);

        if (message == null)
        {
            await handle(message!);
        }

        return this;
    }
}