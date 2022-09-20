using ArquiteturaCamadas.Business.Interfaces.Repositories.RepositoryBase;
using ArquiteturaCamadas.Domain.Entities.EntityBase;
using ArquiteturaCamadas.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Principal;

namespace ArquiteturaCamadas.Infra.Repositories.RepositoryBase
{
    public abstract class BaseQueryCommandsRepository<TEntity> : BaseCommandsRepository<TEntity>, IBaseQueryCommandsRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected BaseQueryCommandsRepository(ArquiteturaCamadasDbContext dbContext) : base(dbContext)
        {
        }

        public virtual async Task<TEntity> FindByIdAsync(int id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null, bool asNoTracking = false)
        {
            var query = (IQueryable<TEntity>)_dbContext.Set<TEntity>();

            if (asNoTracking)
                query = _dbContextSet.AsNoTracking();

            if (includes != null)
                query = includes(query);

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }
        
        public virtual async Task<List<TEntity>> FindAllEntitiesAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            var query = (IQueryable<TEntity>)_dbContextSet;

            if (includes != null)
                query = includes(query);

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
