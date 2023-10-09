using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xprtz.Training.Infra.EfCore.Entities;
using Xprtz.Training.Infra.EfCore.EntityConfigurations;

namespace Xprtz.Training.Infra.EfCore;

public class CacheDbContext : DbContext
{
    private IConfiguration _configuration;
    
    public CacheDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServer"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CommentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PostEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<PostEntity> PostListViewModels { get; set; } = default!;
    public virtual DbSet<CommentEntity> CommentViewModels { get; set; } = default!;
}