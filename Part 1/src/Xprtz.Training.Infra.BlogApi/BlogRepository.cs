using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Xprtz.Training.Domain.Interfaces;
using Xprtz.Training.Domain.Models;
using Xprtz.Training.Infra.BlogApi.Mappers;
using Xprtz.Training.Infra.BlogApi.Models;

namespace Xprtz.Training.Infra.BlogApi;

public class BlogRepository : IBlogRepository
{
    private HttpClient _httpClient;
    
    public BlogRepository(IConfiguration configuration)
    {
        var connectionStringUri = configuration.GetConnectionString("BlogApi") ??
                                  throw new Exception("BlogApi connection string not found.");
        
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(connectionStringUri),
        };
    }
    
    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<PostApiModel>>("posts", GetSerializerOptions());

        return result?.ToDomain() ?? Enumerable.Empty<Post>();
    }

    public async Task<Post?> GetPostByIdAsync(int id, bool includeComments = false)
    {
        var result = await _httpClient.GetFromJsonAsync<PostApiModel>($"posts/{id}", GetSerializerOptions());

        if (result == null)
            return null;
        
        if (includeComments)
        {
            var comments = await _httpClient.GetFromJsonAsync<IEnumerable<CommentApiModel>>($"posts/{id}/comments", GetSerializerOptions());
            result.Comments = comments ?? Enumerable.Empty<CommentApiModel>();
        }

        return result.ToDomain();
    }
    
    private JsonSerializerOptions GetSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }
}