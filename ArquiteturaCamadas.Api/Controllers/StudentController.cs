using ArquiteturaCamadas.Api.ControllersAttributes;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Student;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Student;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArquiteturaCamadas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("add-student")]
        [CommandsResponseTypes]
        public async Task<bool> AddAsync([FromBody] StudentSaveRequest studentSaveRequest) =>
            await _studentService.AddAsync(studentSaveRequest);

        [HttpPut("update-student")]
        [CommandsResponseTypes]
        public async Task<bool> UpdateAsync([FromBody] StudentUpdateRequest studentUpdateRequest) =>
            await _studentService.UpdateAsync(studentUpdateRequest);

        [HttpPut("remove-project")]
        [CommandsResponseTypes]
        public async Task<bool> RemoveProjectAsync([FromQuery] ProjectStudentRelationShipArgument projectStudentRelationShipArgument) =>
            await _studentService.RemoveProjectAsync(projectStudentRelationShipArgument);

        [HttpDelete("delete-student")]
        [CommandsResponseTypes]
        public async Task<bool> DeleteAsync([FromQuery] int id) =>
            await _studentService.DeleteAsync(id);

        [HttpGet("find-student")]
        [QueryCommandsResponseTypes]
        public async Task<StudentResponse> FindByIdAsync([FromQuery] int id) =>
            await _studentService.FindByIdAsync(id);

        [HttpGet("find-all-students")]
        [QueryCommandsResponseTypes]
        public async Task<List<StudentResponse>> FindAllEntitiesAsync() =>
            await _studentService.FindAllEntitiesAsync();

        [HttpGet("find-all-students-paginated")]
        [QueryCommandsResponseTypes]
        public async Task<PageList<StudentResponse>> FindAllEntitiesWithPaginationAsync([FromQuery] PageParams pageParams) =>
            await _studentService.FindAllEntitiesWithPaginationAsync(pageParams);
    }
}
