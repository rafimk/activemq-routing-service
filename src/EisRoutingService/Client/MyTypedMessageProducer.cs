﻿using ActiveMQ.Artemis.Client;

namespace EisRoutingService.Client;

public class MyTypedMessageProducer
{
    private readonly IProducer _producer;

    public MyTypedMessageProducer(IProducer producer)
    {
        _producer = producer;
    }

    public async Task SendTextAsync(string text)
    {
        var message = new Message(text);
        await _producer.SendAsync(message);
    }
}