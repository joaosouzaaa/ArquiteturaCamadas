using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Arguments;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Student;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Student;
using ArquiteturaCamadas.ApplicationService.Interfaces.ServiceBase;

namespace ArquiteturaCamadas.ApplicationService.Interfaces
{
    public interface IStudentService :
                     ICommandsService<StudentSaveRequest, StudentUpdateRequest>,
                     IQueryCommandsService<StudentResponse>
    {
        Task<bool> RemoveProjectAsync(ProjectStudentRelationShipArgument projectStudentRelationShipArgument);
        Task<bool> HaveStudentInDbAsync(int id);
    }
}
