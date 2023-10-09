using Xprtz.Training.Domain.Models;
using Xprtz.Training.Infra.EfCore.Entities;

namespace Xprtz.Training.Infra.EfCore.Mappers;

public static class PostMapper
{
    public static Post ToDomain(this PostEntity entity)
        => Post.FromExisting(entity.Id, entity.UserId, entity.Title, entity.Body, entity.Comments.ToDomain().ToList());
    
    public static IEnumerable<Post> ToDomain(this IEnumerable<PostEntity> entities)
        => entities.Select(ToDomain);
    
    public static PostEntity ToDbModel(this Post post)
        => new PostEntity
        {
            Id = post.Id,
            UserId = post.UserId,
            Title = post.Title,
            Body = post.Body,
            CachedAt = DateTimeOffset.Now
        };

    public static IEnumerable<PostEntity> ToDbModels(this IEnumerable<Post> posts)
        => posts.Select(ToDbModel);
}