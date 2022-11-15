using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Infra.Contexts;
using ArquiteturaCamadas.Infra.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore;

namespace ArquiteturaCamadas.Infra.Repositories
{
    public sealed class PostRepository : BaseQueryCommandsRepository<Post>, IPostRepository
    {
        public PostRepository(ArquiteturaCamadasDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> DetachEntityAndSaveChangesAsync(Post post)
        {
            _dbContext.Entry(post).State = EntityState.Detached;

            return await SaveChangesAsync();
        }
    }
}
