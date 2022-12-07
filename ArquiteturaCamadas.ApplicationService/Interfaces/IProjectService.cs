using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Project;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Project;
using ArquiteturaCamadas.ApplicationService.Interfaces.ServiceBase;
using ArquiteturaCamadas.Domain.Entities;

namespace ArquiteturaCamadas.ApplicationService.Interfaces
{
    public interface IProjectService : ICommandsService<ProjectSaveRequest, ProjectUpdateRequest>
    {
        Task<ProjectResponse> FindProjectByIdAsync(int id);
        Task<List<ProjectResponse>> FindAllProjectsAsync();
    }
}
