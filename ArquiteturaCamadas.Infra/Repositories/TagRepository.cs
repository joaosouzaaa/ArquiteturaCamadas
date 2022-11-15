using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Infra.Contexts;
using ArquiteturaCamadas.Infra.Repositories.RepositoryBase;

namespace ArquiteturaCamadas.Infra.Repositories
{
    public sealed class TagRepository : BaseQueryCommandsRepository<Tag>, ITagRepository
    {
        public TagRepository(ArquiteturaCamadasDbContext dbContext) : base(dbContext)
        {
        }
    }
}
