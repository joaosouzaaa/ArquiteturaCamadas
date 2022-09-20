using ArquiteturaCamadas.Business.Interfaces.Repositories.RepositoryBase;
using ArquiteturaCamadas.Domain.Entities.EntityBase;
using ArquiteturaCamadas.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArquiteturaCamadas.Infra.Repositories.RepositoryBase
{
    public abstract class BaseCommandsRepository<TEntity> : IBaseCommandsRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly ArquiteturaCamadasDbContext _dbContext;
        protected DbSet<TEntity> _dbContextSet => _dbContext.Set<TEntity>();

        protected BaseCommandsRepository(ArquiteturaCamadasDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task<bool> SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync() > 0;

        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            await _dbContextSet.AddAsync(entity);

            return await SaveChangesAsync();
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            return await SaveChangesAsync();
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbContextSet.FirstOrDefaultAsync(e => e.Id == id);

            _dbContextSet.Remove(entity);

            return await SaveChangesAsync();
        }

        public virtual async Task<bool> HaveObjectInDbAsync(Expression<Func<TEntity, bool>> where) => await _dbContextSet.AsNoTracking().AnyAsync(where);

        public void Dispose() => _dbContext.Dispose();
    }
}
