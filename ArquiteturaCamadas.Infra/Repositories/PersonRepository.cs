using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Infra.Contexts;
using ArquiteturaCamadas.Infra.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore;

namespace ArquiteturaCamadas.Infra.Repositories
{
    public sealed class PersonRepository : BaseQueryCommandsRepository<Person>, IPersonRepository
    {
        public PersonRepository(ArquiteturaCamadasDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<bool> UpdateAsync(Person entity)
        {
            _dbContext.Entry(entity.Address).State = EntityState.Modified;

            return await base.UpdateAsync(entity);
        }
    }
}
