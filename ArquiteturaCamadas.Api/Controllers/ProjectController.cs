using ArquiteturaCamadas.Api.ControllersAttributes;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Project;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Project;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArquiteturaCamadas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost("add-project")]
        [CommandsResponseTypes]
        public async Task<bool> AddAsync([FromBody] ProjectSaveRequest projectSaveRequest) =>
            await _projectService.AddAsync(projectSaveRequest);

        [HttpPut("update-project")]
        [CommandsResponseTypes]
        public async Task<bool> UpdateAsync([FromBody] ProjectUpdateRequest projectUpdateRequest) =>
            await _projectService.UpdateAsync(projectUpdateRequest);

        [HttpDelete("delete-project")]
        [CommandsResponseTypes]
        public async Task<bool> DeleteAsync([FromQuery] int id) =>
            await _projectService.DeleteAsync(id);

        [HttpGet("find-project")]
        [QueryCommandsResponseTypes]
        public async Task<ProjectResponse> FindProjectByIdAsync([FromQuery] int id) =>
            await _projectService.FindProjectByIdAsync(id);

        [HttpGet("find-all-projects")]
        [QueryCommandsResponseTypes]
        public async Task<List<ProjectResponse>> FindAllProjectsAsync() =>
            await _projectService.FindAllProjectsAsync();
    }
}
