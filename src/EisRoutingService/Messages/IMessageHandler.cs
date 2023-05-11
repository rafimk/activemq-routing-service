namespace EisRoutingService.Messages;

public interface IMessageHandler<in TMessage> where TMessage : class, IMessage
{
    Task HandleAsync(TMessage message);
}