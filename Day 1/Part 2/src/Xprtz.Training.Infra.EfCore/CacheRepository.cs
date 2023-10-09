using Microsoft.EntityFrameworkCore;
using Xprtz.Training.Domain.Interfaces;
using Xprtz.Training.Domain.Models;
using Xprtz.Training.Infra.EfCore.Mappers;

namespace Xprtz.Training.Infra.EfCore;

public class CacheRepository : ICacheRepository
{
    private readonly CacheDbContext _dbContext;

    public CacheRepository(CacheDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CacheValid()
    {
        // Get a single post 
        var post = await _dbContext.PostListViewModels.FirstOrDefaultAsync();

        // There are no posts
        if (post == null)
            return false;

        // Check if the cache is too old
        if (DateTimeOffset.Now - post.CachedAt > TimeSpan.FromSeconds(60))
        {
            return false;
        }
        
        return true;
    }
    
    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
        var results = await _dbContext.PostListViewModels
            .OrderBy(x => x.Id)
            .ToListAsync();

        return results.ToDomain();
    }

    public async Task<Post?> GetPostByIdAsync(int id, bool includeComments = false)
    {
        var post = await _dbContext.PostListViewModels
            .FirstOrDefaultAsync(x => x.Id == id);

        if (post == null)
            return null;

        if (includeComments)
        {
            var comments = await _dbContext.CommentViewModels
                .Where(x => x.PostId == id)
                .OrderBy(x => x.Id)
                .ToListAsync();

            post.Comments = comments;
        }
        
        return post.ToDomain();
    }

    public async Task InsertPostsAsync(IEnumerable<Post> posts)
    {
        _dbContext.ChangeTracker.Clear();
        _dbContext.PostListViewModels.AddRange(posts.ToDbModels());
        await _dbContext.Database.OpenConnectionAsync();
        await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Posts ON");
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Posts OFF");
    }

    public async Task InsertCommentsAsync(IEnumerable<Comment> comments)
    {
        _dbContext.ChangeTracker.Clear();
        _dbContext.CommentViewModels.AddRange(comments.ToDbModels());
        await _dbContext.Database.OpenConnectionAsync();
        await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Comments ON");
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Comments OFF");
    }

    public async Task DeletePostsAndCommentsAsync()
    {
        await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM dbo.Posts;");
        await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM dbo.Comments;");
    }
}