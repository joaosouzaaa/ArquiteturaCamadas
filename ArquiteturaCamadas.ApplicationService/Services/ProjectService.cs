using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Project;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Project;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Services.ServiceBase;
using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ArquiteturaCamadas.ApplicationService.Services
{
    public sealed class ProjectService : BaseService<Project>, IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IStudentService _studentService;

        public ProjectService(IProjectRepository projectRepository, IStudentService studentService,
                              IValidator<Project> validator, INotificationHandler notification)
                              : base(validator, notification)
        {
            _projectRepository = projectRepository;
            _studentService = studentService;
        }

        public async Task<bool> AddAsync(ProjectSaveRequest projectSaveRequest)
        {
            if (!await _studentService.HaveStudentInDbAsync(projectSaveRequest.StudentId))
                return _notification.AddDomainNotification(EMessage.NotFound.ToString(), EMessage.NotFound.Description().FormatTo("Student"));

            var project = projectSaveRequest.MapTo<ProjectSaveRequest, Project>();

            if (!await ValidateAsync(project))
                return false;

            return await _projectRepository.AddAsync(project);
        }

        public async Task<bool> UpdateAsync(ProjectUpdateRequest projectUpdateRequest)
        {
            var project = await _projectRepository.FindByIdAsync(projectUpdateRequest.Id, p => p.Include(p => p.Student), true);

            if(project is null)
                return _notification.AddDomainNotification(EMessage.NotFound.ToString(), EMessage.NotFound.Description().FormatTo("Project"));

            if (!await _studentService.HaveStudentInDbAsync(projectUpdateRequest.StudentId))
                return _notification.AddDomainNotification(EMessage.NotFound.ToString(), EMessage.NotFound.Description().FormatTo("Student"));

            project = projectUpdateRequest.MapTo<ProjectUpdateRequest, Project>();

            if (!await ValidateAsync(project))
                return false;

            return await _projectRepository.UpdateAsync(project);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if(!await _projectRepository.HaveObjectInDbAsync(p => p.Id == id))
                return _notification.AddDomainNotification(EMessage.NotFound.ToString(), EMessage.NotFound.Description().FormatTo("Project"));

            return await _projectRepository.DeleteAsync(id);
        }

        public async Task<ProjectResponse> FindProjectByIdAsync(int id)
        {
            var project = await _projectRepository.FindByIdAsync(id, null, true);

            return project.MapTo<Project, ProjectResponse>();
        }

        public async Task<List<ProjectResponse>> FindAllProjectsAsync()
        {
            var projectsList = await _projectRepository.FindAllEntitiesAsync();

            return projectsList.MapTo<List<Project>, List<ProjectResponse>>();
        }
    }
}
