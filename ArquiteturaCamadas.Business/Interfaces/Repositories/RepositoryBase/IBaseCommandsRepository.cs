using ArquiteturaCamadas.Domain.Entities.EntityBase;
using System.Linq.Expressions;

namespace ArquiteturaCamadas.Business.Interfaces.Repositories.RepositoryBase
{
    public interface IBaseCommandsRepository<TEntity>: IDisposable
        where TEntity : BaseEntity 
    {
        Task<bool> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> HaveObjectInDbAsync(Expression<Func<TEntity, bool>> where);
    }
}
