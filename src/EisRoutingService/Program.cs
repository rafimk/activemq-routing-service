
using ActiveMQ.Artemis.Client;
using ActiveMQ.Artemis.Client.AutoRecovering.RecoveryPolicy;
using ActiveMQ.Artemis.Client.Extensions.DependencyInjection;
using ActiveMQ.Artemis.Client.Extensions.Hosting;
using ActiveMQ.Artemis.Client.MessageIdPolicy;
using Amqp;
using EisRoutingService.Client;
using EisRoutingService.Consumers;
using EisRoutingService.Contracts;
using EisRoutingService.Messages;
using EisRoutingService.Publishers;
using EisRoutingService.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.Contracts;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


 builder.Services.AddMessaging();

// builder.Services.AddHostedService<MessagingBackgroundService>();
builder.Services.AddActiveMq(name: "my-artemis-cluster", endpoints: new[] { ActiveMQ.Artemis.Client.Endpoint.Create(host: "localhost", port: 5672, "admin", "admin") })
                 .ConfigureConnectionFactory((provider, factory) =>
                 {
                     factory.LoggerFactory = provider.GetService<ILoggerFactory>();
                     factory.RecoveryPolicy = RecoveryPolicyFactory.ExponentialBackoff(initialDelay: TimeSpan.FromSeconds(1), maxDelay: TimeSpan.FromSeconds(30), retryCount: 5);
                     factory.MessageIdPolicyFactory = MessageIdPolicyFactory.GuidMessageIdPolicy;
                     factory.AutomaticRecoveryEnabled = true;
                 })
                 .ConfigureConnection((_, connection) =>
                 {
                     connection.ConnectionClosed += (_, args) =>
                     {
                         Console.WriteLine($"Connection closed: ClosedByPeer={args.ClosedByPeer}, Error={args.Error}");
                     };
                     connection.ConnectionRecovered += (_, args) =>
                     {
                         Console.WriteLine($"Connection recovered: Endpoint={args.Endpoint}");
                     };
                     connection.ConnectionRecoveryError += (_, args) =>
                     {
                         Console.WriteLine($"Connection recovered error: Exception={args.Exception}");
                     };
                 })
                 .AddConsumer("topic.outbound.book", RoutingType.Multicast, "topic.outbound.book", 
                 async (message, consumer, token, serviceProvider) =>
                 {
                     var body = message.GetBody<string>();
                     // Console.WriteLine("topic.outbound.test: " + body);

                     ILogger logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
                     var dispatcher = builder.Services.BuildServiceProvider().GetRequiredService<IMessageDispatcher>();

                     var msg = JsonSerializer.Deserialize<BookCreated>(body);

                     if (msg is not null)
                     {
                         await dispatcher.DispatchAsync(msg!);
                     }
                     
                     // logger.LogInformation($"Log Message : {body}");

                     await consumer.AcceptAsync(message);

                 })
                // .AddTypedConsumer<BookCreated, BookCreatedConsumer>(RoutingType.Multicast, "topic.inbound.test")
                 .AddProducer<MyTypedMessageProducer>("topic.outbound.book", RoutingType.Multicast)
                 .EnableQueueDeclaration()
                 .EnableAddressDeclaration();

            builder.Services.AddActiveMqHostedService();
  

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
