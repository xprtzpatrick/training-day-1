using Xprtz.Training.Domain.Models;

namespace Xprtz.Training.Domain.Interfaces;

public interface IBlogRepository
{
    Task<IEnumerable<Post>> GetPostsAsync();
    Task<Post?> GetPostByIdAsync(int id, bool includeComments = false);
}