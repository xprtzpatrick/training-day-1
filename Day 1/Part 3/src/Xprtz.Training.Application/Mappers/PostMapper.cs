using Xprtz.Training.Application.Models;
using Xprtz.Training.Domain.Models;

namespace Xprtz.Training.Application.Mappers;

public static class PostMapper
{
    public static PostListViewResponse ToViewModelForList(this Post post)
        => new PostListViewResponse(post.Id, post.UserId, post.Title, post.Body);
    
    public static PostResponse ToViewModelForSingle(this Post post)
        => new PostResponse(post.Id, post.UserId, post.Title, post.Body, post.Comments.Select(x => x.ToViewModel()).ToList());
}