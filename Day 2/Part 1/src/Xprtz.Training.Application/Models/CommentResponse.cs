namespace Xprtz.Training.Application.Models;

public record CommentResponse(int Id, int PostId, string Name, string Email, string Body);