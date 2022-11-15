using ArquiteturaCamadas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArquiteturaCamadas.Infra.EntitiesMapping
{
    public sealed class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(a => a.ZipCode).HasColumnType("char(8)")
                .HasColumnName("zip_code").IsRequired(true);

            builder.Property(a => a.District).HasColumnType("varchar(50)")
                .HasColumnName("district").IsRequired(true);

            builder.Property(a => a.Complement).HasColumnType("varchar(50)")
                .HasColumnName("complement").IsRequired(false);

            builder.Property(a => a.Number).HasColumnType("varchar(10)")
                .HasColumnName("number").IsRequired(true);

            builder.Property(a => a.Street).HasColumnType("varchar(50)")
                .HasColumnName("street").IsRequired(true);

            builder.Property(a => a.City).HasColumnType("varchar(50)")
                .HasColumnName("city").IsRequired(true);

            builder.Property(a => a.State).HasColumnType("char(2)")
                .HasColumnName("state").IsRequired(true);
        }
    }
}
