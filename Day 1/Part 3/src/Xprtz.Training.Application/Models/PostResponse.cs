namespace Xprtz.Training.Application.Models;

public record PostResponse(int Id, int UserId, string Title, string Body, List<CommentResponse> Comments);
