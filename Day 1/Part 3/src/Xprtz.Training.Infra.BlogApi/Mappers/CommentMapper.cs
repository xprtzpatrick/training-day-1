using Xprtz.Training.Domain.Models;
using Xprtz.Training.Infra.BlogApi.Models;

namespace Xprtz.Training.Infra.BlogApi.Mappers;

public static class CommentMapper
{
    public static Comment ToDomain(this CommentApiModel comment)
        => Comment.FromExisting(comment.Id, comment.PostId, comment.Name, comment.Email, comment.Body);
    
    public static IEnumerable<Comment> ToDomain(this IEnumerable<CommentApiModel> comments)
        => comments.Select(ToDomain);
}