using ArquiteturaCamadas.Business.Interfaces.Repositories.RepositoryBase;
using ArquiteturaCamadas.Domain.Entities;

namespace ArquiteturaCamadas.Business.Interfaces.Repositories
{
    public interface IProjectRepository : 
                     IBaseCommandsRepository<Project>,
                     IBaseQueryCommandsRepository<Project>
    {
    }
}
