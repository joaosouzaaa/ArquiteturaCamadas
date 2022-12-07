using ArquiteturaCamadas.Business.Interfaces.Repositories.RepositoryBase;
using ArquiteturaCamadas.Domain.Entities;

namespace ArquiteturaCamadas.Business.Interfaces.Repositories
{
    public interface IStudentRepository :
                     IBaseCommandsRepository<Student>,
                     IBaseQueryCommandsRepository<Student>
    {
        Task<bool> UpdateRelationshipAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
    }
}
