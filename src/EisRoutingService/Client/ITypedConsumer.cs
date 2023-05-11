namespace EisRoutingService.Client;

public interface ITypedConsumer<in T>
{
    public Task ConsumeAsync(T message, CancellationToken cancellationToken);
}