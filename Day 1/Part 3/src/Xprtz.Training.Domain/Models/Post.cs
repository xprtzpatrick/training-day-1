using Xprtz.Training.Domain.Exceptions;

namespace Xprtz.Training.Domain.Models;

public class Post
{
    public int Id { get; }
    public int UserId { get; internal set; }
    public string Title { get; internal set; } = string.Empty;
    public string Body { get; internal set; } = string.Empty;
    public List<Comment> Comments { get; } = new();
    
    private Post(int id, int userId, string title, string body, List<Comment> comments)
    {
        Id = id;
        Comments = comments;
        SetUserId(userId);
        SetTitle(title);
        SetBody(body);
    }
    
    public static Post FromExisting(int id, int userId, string title, string body, List<Comment> comments)
        => new(id, userId, title, body, comments);
    
    public static Post Create(int userId, string title, string body, List<Comment> comments)
        => new(0, userId, title, body, comments);

    public Post SetUserId(int id)
    {
        if (id == 0)
            throw new InvalidUserIdDomainException();

        UserId = id;
        return this;
    }
    
    public Post SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidPostTitleDomainException();
        
        Title = title;
        return this;
    }
    
    public Post SetBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            throw new InvalidPostBodyDomainException();
        
        Body = body;
        return this;
    }
}