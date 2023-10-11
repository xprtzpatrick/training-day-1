using Xprtz.Training.MicroServices.Domain.Interfaces;
using Xprtz.Training.MicroServices.Domain.Models;

namespace Xprtz.Training.MicroServices.Domain;

public class ChatService : IChatService
{
    private readonly ChatServiceConfig _config;

    public ChatService(ChatServiceConfig config)
    {
        _config = config;
    }
    
    public void HandleChatMessage(List<string> currentMessages, string message, string owner)
    {
        // We leave this message alone if it's not ours
        if (_config.Owner != owner)
            return;

        if (message == "clear")
        {
            currentMessages.Clear();
            return;
        }

        currentMessages.Add(message);
    }
}