namespace EisRoutingService.Messages;

public interface IMessagingConfiguration
{
    IServiceCollection Services { get; }
}

public record MessagingConfiguration(IServiceCollection Services) : IMessagingConfiguration;