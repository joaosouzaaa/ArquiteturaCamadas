using ArquiteturaCamadas.Business.Interfaces.Repositories.RepositoryBase;
using ArquiteturaCamadas.Domain.Entities;

namespace ArquiteturaCamadas.Business.Interfaces.Repositories
{
    public interface IPostRepository : IBaseQueryCommandsRepository<Post>
    {
        Task<bool> DetachEntityAndSaveChangesAsync(Post post);
    }
}
