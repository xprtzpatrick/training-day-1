using Xprtz.Training.Domain.Models;
using Xprtz.Training.Infra.EfCore.Entities;

namespace Xprtz.Training.Infra.EfCore.Mappers;

public static class CommentMapper
{
    public static Comment ToDomain(this CommentEntity entity)
        => Comment.FromExisting(entity.Id, entity.PostId, entity.Name, entity.Email, entity.Body);
    
    public static IEnumerable<Comment> ToDomain(this IEnumerable<CommentEntity> entities)
        => entities.Select(ToDomain);
    
    public static CommentEntity ToDbModel(this Comment comment)
        => new CommentEntity
        {
            Id = comment.Id,
            PostId = comment.PostId,
            Name = comment.Name,
            Email = comment.Email,
            Body = comment.Body
        };
    
    public static IEnumerable<CommentEntity> ToDbModels(this IEnumerable<Comment> comments)
        => comments.Select(ToDbModel);
}