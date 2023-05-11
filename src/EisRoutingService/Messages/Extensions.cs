using ActiveMQ.Artemis.Client;
using EisRoutingService.Accessors;
using EisRoutingService.Connections;
using EisRoutingService.Publishers;
using EisRoutingService.Subscribers;

namespace EisRoutingService.Messages;

public static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        var endpoint = ActiveMQ.Artemis.Client.Endpoint.Create(host: "localhost", port: 5672, "admin", "admin");
        var connectionFactory = new ConnectionFactory();
        var connection = connectionFactory.CreateAsync(endpoint).Result;

        services.AddSingleton(connection);
        services.AddSingleton<ChannelAccessor>();
        services.AddSingleton<IChannelFactory, ChannelFactory>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        services.AddSingleton<IMessageSubscriber, MessageSubscriber>();
        services.AddSingleton<IMessageDispatcher, MessageDispatcher>();
        services.AddSingleton<IMessageIdAccessor, MessageIdAccessor>();

        services.Scan(cfg => cfg.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(IMessageHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}