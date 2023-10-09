using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Xprtz.Training.MicroServices.Api.Controllers;
using Xprtz.Training.MicroServices.Models;

namespace Xprtz.Training.MicroServices.Api;

public class ServiceBusService : IHostedService
{
    private string _owner;
    private ServiceBusProcessor _processor;
    
    public ServiceBusService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ServiceBus") ?? throw new Exception("Could not start servicebus, missing connectionstring?");
        _owner = configuration.GetSection("ServiceBus")["owner"] ?? throw new Exception("No owner set!");
        
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

    private bool HandleMessage(ServiceBusReceivedMessage message)
    {
        // We leave this message alone if it's not ours
        if ((string) message.ApplicationProperties["owner"] != _owner)
            return false;

        var chatMessage = JsonSerializer.Deserialize<ChatMessage>(message.Body.ToString());
        if (chatMessage == null)
            throw new Exception("Body could not be decoded into a chatmessage model!");

        if (chatMessage.Message == "clear")
        {
            ApiController.Messages.Clear();
            return true;
        }

        ApiController.Messages.Add(chatMessage.Message);
        return true;
    }

    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}