using System.Text.Json.Serialization;

namespace Xprtz.Training.Infra.BlogApi.Models;

public class PostApiModel
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public IEnumerable<CommentApiModel> Comments { get; set; } = Enumerable.Empty<CommentApiModel>();
}