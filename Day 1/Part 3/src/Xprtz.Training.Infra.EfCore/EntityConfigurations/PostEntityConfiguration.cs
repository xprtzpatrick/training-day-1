using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xprtz.Training.Infra.EfCore.Entities;

namespace Xprtz.Training.Infra.EfCore.EntityConfigurations;

public class PostEntityConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.CachedAt);
        builder.ToTable("Posts");
    }
}