using System.Net.Mail;
using Xprtz.Training.Domain.Exceptions;

namespace Xprtz.Training.Domain.Models;

public sealed class Comment
{
    public int Id { get; }
    public int PostId { get; }
    public string Name { get; internal set; } = string.Empty;
    public string Email { get; internal set; } = string.Empty;
    public string Body { get; internal set; } = string.Empty;

    private Comment(int id, int postId, string name, string email, string body)
    {
        Id = id;
        PostId = postId;
        SetName(name);
        SetBody(body);
        SetEmail(email);
    }
    
    public static Comment FromExisting(int id, int postId, string name, string email, string body)
        => new Comment(id, postId, name, email, body);
    
    public static Comment Create(int postId, string name, string email, string body)
        => new Comment(0, postId, name, email, body);

    public Comment SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidNameDomainException();
        
        Name = name;
        return this;
    }

    public Comment SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidEmailDomainException();

        try
        {
            _ = new MailAddress(email);
        }
        catch
        {
            throw new InvalidEmailDomainException();
        }
        
        Email = email;
        return this;
    }

    public Comment SetBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            throw new InvalidCommentBodyDomainException();

        Body = body;
        return this;
    }
}