using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xprtz.Training.Infra.EfCore.Entities;

namespace Xprtz.Training.Infra.EfCore.EntityConfigurations;

public class CommentEntityConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.PostId);
        builder.ToTable("Comments");
    }
}