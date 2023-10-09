using Xprtz.Training.Domain.Models;

namespace Xprtz.Training.Domain.Interfaces;

public interface ICacheRepository
{
    Task<DateTimeOffset?> GetLastCacheUpdateTimeAsync();
    Task<IEnumerable<Post>> GetPostsAsync();
    Task<Post?> GetPostByIdAsync(int id, bool includeComments = false);
    Task InsertPostsAsync(IEnumerable<Post> posts);
    Task InsertCommentsAsync(IEnumerable<Comment> comments);
    Task DeletePostsAndCommentsAsync();
}