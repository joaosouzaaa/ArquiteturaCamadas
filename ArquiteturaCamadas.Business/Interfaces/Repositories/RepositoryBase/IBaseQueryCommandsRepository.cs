using ArquiteturaCamadas.Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore.Query;

namespace ArquiteturaCamadas.Business.Interfaces.Repositories.RepositoryBase
{
    public interface IBaseQueryCommandsRepository<TEntity> : IBaseCommandsRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<TEntity> FindByIdAsync(int id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null, bool asNoTracking = false);
        Task<List<TEntity>> FindAllEntitiesAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);
    }
}
