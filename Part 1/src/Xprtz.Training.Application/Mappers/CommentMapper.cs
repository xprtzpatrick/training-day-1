using Xprtz.Training.Application.Models;
using Xprtz.Training.Domain.Models;

namespace Xprtz.Training.Application.Mappers;

public static class CommentMapper
{
    public static CommentResponse ToViewModel(this Comment comment)
        => new CommentResponse(comment.Id, comment.PostId, comment.Name, comment.Email, comment.Body);
    
    public static IEnumerable<CommentResponse> ToViewModel(this IEnumerable<Comment> comments)
        => comments.Select(ToViewModel);
}