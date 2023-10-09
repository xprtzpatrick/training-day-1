using Xprtz.Training.Domain.Interfaces;
using Xprtz.Training.Domain.Models;

namespace Xprtz.Training.Domain.Services;

public class BlogService
{
    private readonly IBlogRepository _blogRepository;
    private readonly ICacheRepository _cacheRepository;

    public BlogService(IBlogRepository blogRepository, ICacheRepository cacheRepository)
    {
        _blogRepository = blogRepository;
        _cacheRepository = cacheRepository;
    }

    private async Task PopulateCache()
    {
        if (await _cacheRepository.CacheValid())
            return;
        
        // Delete the old cache
        await _cacheRepository.DeletePostsAndCommentsAsync();
        
        Console.WriteLine("Fetching posts from API");
        var posts = (await _blogRepository.GetPostsAsync()).ToList();

        await _cacheRepository.InsertPostsAsync(posts);
        
        // Get all comments and insert into the database
        await _cacheRepository.InsertCommentsAsync(await _blogRepository.GetAllComments());
    }

    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
        await PopulateCache();
        Console.WriteLine("Fetching posts from cache");
        return (await _cacheRepository.GetPostsAsync()).ToList();
    }

    public async Task<Post?> GetPostByIdAsync(int id, bool includeComments = false)
    {
        await PopulateCache();
        Console.WriteLine($"Fetching post {id} from cache");
        return await _cacheRepository.GetPostByIdAsync(id, includeComments);
    }
}