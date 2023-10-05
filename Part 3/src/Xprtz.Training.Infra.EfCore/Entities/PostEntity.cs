namespace Xprtz.Training.Infra.EfCore.Entities;

public class PostEntity
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public IEnumerable<CommentEntity> Comments { get; set; } = Enumerable.Empty<CommentEntity>();
    public DateTimeOffset CachedAt { get; set; }
}