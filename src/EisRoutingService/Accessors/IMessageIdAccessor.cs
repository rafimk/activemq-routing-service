namespace EisRoutingService.Accessors;

public interface IMessageIdAccessor
{
    string GetMessageId();
    void SetMessageId(string messageId);
}