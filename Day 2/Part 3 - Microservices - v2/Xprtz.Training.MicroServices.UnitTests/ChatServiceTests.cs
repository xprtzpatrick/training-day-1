using FluentAssertions;
using Xprtz.Training.MicroServices.Domain;
using Xprtz.Training.MicroServices.Domain.Models;

namespace Xprtz.Training.MicroServices.UnitTests;

public class ChatServiceTests
{
    [Fact]
    public void WithCorrectOwner_ShouldHandleMessage()
    {
        // Arrange
        const string owner = "Test";
        var chatService = GetChatServiceWithOwner(owner);
        
        // Act
        var result = chatService.ShouldHandleMessage(new Dictionary<string, object>
        {
            {"owner", owner}
        });

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void WithIncorrectOwner_ShouldNotHandleMessage()
    {
        // Arrange
        var chatService = GetChatServiceWithOwner("AnOwner");
        
        // Act
        var result = chatService.ShouldHandleMessage(new Dictionary<string, object>
        {
            {"owner", "AnotherOwner"}
        });

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void WithMessageClear_ShouldClearList()
    {
        // Arrange
        var chatService = GetChatServiceWithOwner();
        var currentMessages = new List<string>
        {
            "Message 1",
            "Message 2",
            "Message 3"
        };

        // Act
        chatService.HandleChatMessage(currentMessages, "clear");

        // Assert
        currentMessages.Should().BeEmpty();
    }
    
    [Fact]
    public void WithMessage_ShouldAddToList()
    {
        // Arrange
        const string newMessage = "Message 4";
        var chatService = GetChatServiceWithOwner();
        var currentMessages = new List<string>
        {
            "Message 1",
            "Message 2",
            "Message 3"
        };

        // Act
        chatService.HandleChatMessage(currentMessages, newMessage);

        // Assert
        currentMessages.Should().NotBeEmpty();
        currentMessages.Last().Should().Be(newMessage);
    }
    
    private ChatService GetChatServiceWithOwner(string owner = "owner")
    {
        return new ChatService(new ChatServiceConfig(owner));
    }
}