using ArquiteturaCamadas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArquiteturaCamadas.Infra.Contexts
{
    public class ArquiteturaCamadasDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public ArquiteturaCamadasDbContext(DbContextOptions<ArquiteturaCamadasDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArquiteturaCamadasDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType()
                    .GetProperty("RegistrationDate") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("RegistrationDate").CurrentValue = DateTime.Now;

                else if (entry.State == EntityState.Modified)
                    entry.Property("RegistrationDate").IsModified = false;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
