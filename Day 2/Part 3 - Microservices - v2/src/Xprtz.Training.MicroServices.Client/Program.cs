using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Xprtz.Training.MicroServices.Domain.Models;

ServiceBusClient client;
ServiceBusSender sender;

var queueName = "my-queue-1";
var owner = "Patrick";
var connectionString =
    "Endpoint=sb://xprtz-training.servicebus.windows.net/;SharedAccessKeyName=WriteReadPolicy;SharedAccessKey=2U1ltkPQKjgzKmrcpCnkYWermHwKTHw6i+ASbP06dpY=";

var clientOptions = new ServiceBusClientOptions
{
    TransportType = ServiceBusTransportType.AmqpWebSockets
};

client = new ServiceBusClient(connectionString, clientOptions);
sender = client.CreateSender(queueName);

while (true)
{
    Console.Write("Type a message to send to the queue and press Enter:");
    var message = Console.ReadLine();
    Console.WriteLine();

    if (string.IsNullOrEmpty(message))
    {
        Console.WriteLine("Error: message was empty!");
        continue;
    }

    Console.WriteLine("Sending message!");

    // Send the message
    try
    {
        var chatMessage = new ChatMessage(message);
        var sbMessage = new ServiceBusMessage(JsonSerializer.Serialize(chatMessage));
        sbMessage.ApplicationProperties.Add("owner", owner);

        await sender.SendMessageAsync(sbMessage);
        Console.WriteLine($"Message sent to {queueName} with owner {owner}!");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Could not send message to queue: {queueName} ({e.Message}");
    }
}
