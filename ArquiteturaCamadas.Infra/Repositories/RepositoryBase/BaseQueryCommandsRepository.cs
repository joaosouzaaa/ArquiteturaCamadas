using ArquiteturaCamadas.Business.Interfaces.Repositories.RepositoryBase;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities.EntityBase;
using ArquiteturaCamadas.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics;
using System.Drawing.Printing;
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
            var query = (IQueryable<TEntity>)_dbContextSet;

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

        public virtual async Task<PageList<TEntity>> FindAllEntitiesWithPaginationAsync(PageParams pageParams, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            var query = (IQueryable<TEntity>)_dbContextSet;
            
            if (include != null)
                query = include(query);

            var count = await query.CountAsync();
            var items = await query.Skip((pageParams.PageNumber - 1) * pageParams.PageSize).Take(pageParams.PageSize).ToListAsync();

            return new PageList<TEntity>(items, count, pageParams);
        }
    }
}
