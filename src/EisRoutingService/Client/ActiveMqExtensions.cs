﻿using ActiveMQ.Artemis.Client.Extensions.DependencyInjection;
using ActiveMQ.Artemis.Client;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EisRoutingService.Client;

public static class ActiveMqExtensions
{
    public static IActiveMqBuilder AddTypedConsumer<TMessage, TConsumer>(this IActiveMqBuilder builder,
        RoutingType routingType, string topic)
        where TConsumer : class, ITypedConsumer<TMessage>
    {
        builder.Services.TryAddScoped<TConsumer>();
        builder.AddConsumer(topic, routingType, HandleMessage<TMessage, TConsumer>);
        return builder;
    }

    private static async Task HandleMessage<TMessage, TConsumer>(Message message,
        IConsumer consumer,
        IServiceProvider serviceProvider,
        CancellationToken token)
        where TConsumer : class, ITypedConsumer<TMessage>
    {
        var msg = JsonSerializer.Deserialize<TMessage>(message.GetBody<string>());
        using var scope = serviceProvider.CreateScope();
        var typedConsumer = scope.ServiceProvider.GetService<TConsumer>();
         await typedConsumer.ConsumeAsync(msg, token);
        await consumer.AcceptAsync(message);
    }
}