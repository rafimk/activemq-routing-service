using ActiveMQ.Artemis.Client;

namespace EisRoutingService.Connections;

public interface IChannelFactory
{
    IConnection Create();
}
