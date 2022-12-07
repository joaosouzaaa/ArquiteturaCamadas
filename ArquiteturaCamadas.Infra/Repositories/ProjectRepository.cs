using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Infra.Contexts;
using ArquiteturaCamadas.Infra.Repositories.RepositoryBase;

namespace ArquiteturaCamadas.Infra.Repositories
{
    public sealed class ProjectRepository : BaseQueryCommandsRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ArquiteturaCamadasDbContext dbContext) : base(dbContext)
        {
        }
    }
}
