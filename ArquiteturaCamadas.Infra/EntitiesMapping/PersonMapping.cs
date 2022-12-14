using ArquiteturaCamadas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArquiteturaCamadas.Infra.EntitiesMapping
{
    public sealed class PersonMapping : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name).HasColumnType("varchar(50)")
                .HasColumnName("name").IsRequired(true);

            builder.Property(p => p.Age).HasColumnType("int")
                .HasColumnName("age").IsRequired(true);

            builder.Property(p => p.Gender)
                .HasColumnName("gender").IsRequired(true);

            builder.HasOne(p => p.Address)
                .WithOne()
                .HasForeignKey<Person>(p => p.AddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
