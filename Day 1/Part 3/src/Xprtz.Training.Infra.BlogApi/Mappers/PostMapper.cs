using Xprtz.Training.Domain.Models;
using Xprtz.Training.Infra.BlogApi.Models;

namespace Xprtz.Training.Infra.BlogApi.Mappers;

public static class PostMapper
{
    public static Post ToDomain(this PostApiModel apiModel)
        => Post.FromExisting(
            apiModel.Id, 
            apiModel.UserId, 
            apiModel.Title, 
            apiModel.Body, 
            apiModel.Comments.ToDomain().ToList()
        );
    
    public static IEnumerable<Post> ToDomain(this IEnumerable<PostApiModel> apiModels)
        => apiModels.Select(ToDomain);
}