using Xprtz.Training.MicroServices.Domain.Interfaces;
using Xprtz.Training.MicroServices.Domain.Models;

namespace Xprtz.Training.MicroServices.Domain;

public class ChatService : IChatService
{
    private readonly ChatServiceConfig _config;
    public const string OwnerKey = "owner";

    public ChatService(ChatServiceConfig config)
    {
        _config = config;
    }
    
    public void HandleChatMessage(List<string> currentMessages, string message)
    {
        if (message == "clear")
        {
            currentMessages.Clear();
            return;
        }

        currentMessages.Add(message);
    }

    public bool ShouldHandleMessage(IReadOnlyDictionary<string, object> messageProperties)
    {
        if (!messageProperties.ContainsKey(OwnerKey))
            return false;

        return (string)messageProperties[OwnerKey] == _config.Owner;
    }
}