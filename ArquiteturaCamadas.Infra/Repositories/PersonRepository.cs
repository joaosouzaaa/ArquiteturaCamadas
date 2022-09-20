using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Infra.Contexts;
using ArquiteturaCamadas.Infra.Repositories.RepositoryBase;

namespace ArquiteturaCamadas.Infra.Repositories
{
    public class PersonRepository : BaseQueryCommandsRepository<Person>, IPersonRepository
    {
        public PersonRepository(ArquiteturaCamadasDbContext dbContext) : base(dbContext)
        {
        }
    }
}
