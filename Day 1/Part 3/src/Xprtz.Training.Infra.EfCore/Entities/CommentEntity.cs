namespace Xprtz.Training.Infra.EfCore.Entities;

public class CommentEntity
{
    public int PostId { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}