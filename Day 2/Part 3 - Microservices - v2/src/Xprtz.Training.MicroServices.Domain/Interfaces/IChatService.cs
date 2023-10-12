namespace Xprtz.Training.MicroServices.Domain.Interfaces;

public interface IChatService
{
    void HandleChatMessage(List<string> currentMessages, string message);
    
    bool ShouldHandleMessage(IReadOnlyDictionary<string, object> messageProperties);
}