using ArquiteturaCamadas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArquiteturaCamadas.Infra.EntitiesMapping
{
    public sealed class PostMapping : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Message).HasColumnType("varchar(600)")
                .HasColumnName("message").IsRequired(true);

            builder.Property(p => p.ImageBytes).HasColumnType("varbinary(max)")
                .HasColumnName("image_bytes").IsRequired(false);
        }
    }
}
