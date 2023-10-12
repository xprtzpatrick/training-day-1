using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Xprtz.Training.MicroServices.Api.Controllers;
using Xprtz.Training.MicroServices.Domain.Interfaces;
using Xprtz.Training.MicroServices.Domain.Models;

namespace Xprtz.Training.MicroServices.Api;

public class ServiceBusService : IHostedService
{
    private readonly IChatService _chatService;
    private ServiceBusProcessor _processor;

    public ServiceBusService(IConfiguration configuration, IChatService chatService)
    {
        _chatService = chatService;
        var connectionString = configuration.GetConnectionString("ServiceBus") ??
                               throw new Exception("Could not start servicebus, missing connectionstring?");

        var clientOptions = new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };
        var client = new ServiceBusClient(connectionString, clientOptions);

        _processor = client.CreateProcessor(configuration.GetSection("ServiceBus")["QueueName"]);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            await _processor.StartProcessingAsync(cancellationToken);

            Console.WriteLine("Processor started!");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not start processor! {e.Message}");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
    }

    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        if (args.Message.ApplicationProperties.ContainsKey("owner"))
            HandleMessage(args.Message);

        await args.CompleteMessageAsync(args.Message);
    }

    private void HandleMessage(ServiceBusReceivedMessage message)
    {
        var chatMessage = JsonSerializer.Deserialize<ChatMessage>(message.Body.ToString());
        if (chatMessage == null)
            throw new Exception("Body could not be decoded into a chatmessage model!");

        if (!_chatService.ShouldHandleMessage(message.ApplicationProperties))
            return;
        
        _chatService.HandleChatMessage(ApiController.Messages, chatMessage.Message);
    }

    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}