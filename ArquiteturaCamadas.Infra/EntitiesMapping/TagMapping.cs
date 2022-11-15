using ArquiteturaCamadas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArquiteturaCamadas.Infra.EntitiesMapping
{
    public sealed class TagMapping : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(t => t.TagName).HasColumnType("varchar(50)")
                .HasColumnName("tag_name").IsRequired(true).IsUnicode(true);

            builder.HasMany(t => t.Posts).WithMany(p => p.Tags)
                .UsingEntity<Dictionary<string, object>>("TagsPosts", config =>
                {
                    config.HasOne<Tag>().WithMany().HasForeignKey("TagId");
                    config.HasOne<Post>().WithMany().HasForeignKey("PostId");
                });
        }
    }
}
