using EisRoutingService.Messages;

namespace EisRoutingService.Subscribers;

public interface IMessageSubscriber
{
    Task<IMessageSubscriber> SubscribeMessage<TMessage>(string queue, string routingKey, string exchange,
        Func<TMessage, Task> handle) where TMessage : class, IMessage;
}