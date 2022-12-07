using ArquiteturaCamadas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArquiteturaCamadas.Infra.EntitiesMapping
{
    public sealed class ProjectMapping : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name).HasColumnType("varchar(50)")
                .HasColumnName("name").IsRequired(true);

            builder.Property(p => p.Value).HasColumnType("decimal(18, 2)")
                .HasColumnName("value").IsRequired(true);

            builder.Property(p => p.ExpiryDate).HasColumnType("datetime2")
                .HasColumnName("expiry_date").IsRequired(true);
        }
    }
}
