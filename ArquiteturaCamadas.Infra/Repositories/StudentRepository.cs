using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Infra.Contexts;
using ArquiteturaCamadas.Infra.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore;

namespace ArquiteturaCamadas.Infra.Repositories
{
    public sealed class StudentRepository : BaseQueryCommandsRepository<Student>, IStudentRepository
    {
        public StudentRepository(ArquiteturaCamadasDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<bool> UpdateAsync(Student student)
        {
            _dbContext.Entry(student.Address).State = EntityState.Modified;

            return await base.UpdateAsync(student);
        }
        
        public async Task<bool> UpdateRelationshipAsync(Student student)
        {
            _dbContextSet.Update(student);

            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _dbContextSet.Include(s => s.Address).Include(s => s.Projects).FirstOrDefaultAsync(s => s.Id == id);

            _dbContextSet.Remove(student);

            return await SaveChangesAsync();
        }
    }
}
