using ActiveMQ.Artemis.Client;


namespace EisRoutingService.Connections;

public sealed class ChannelFactory : IChannelFactory
{
    private readonly ChannelAccessor _channelAccessor;

    public ChannelFactory(ChannelAccessor channelAccessor)
    {
        _channelAccessor = channelAccessor;
    }

    IConnection IChannelFactory.Create()
    {
        if (_channelAccessor.Channel is not null)
        {
            return _channelAccessor.Channel;
        }

        var endpoint = ActiveMQ.Artemis.Client.Endpoint.Create(host: "localhost", port: 5672, "admin", "admin");
        var connectionFactory = new ConnectionFactory();
        var connection = connectionFactory.CreateAsync(endpoint).Result;

        return connection;
    }
}
