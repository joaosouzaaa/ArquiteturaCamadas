using ArquiteturaCamadas.Api.ControllersAttributes;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using Microsoft.AspNetCore.Mvc;

namespace ArquiteturaCamadas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost("add-person")]
        [CommandsResponseTypes]
        public async Task<bool> AddAsync([FromBody] PersonSaveRequest personSaveRequest) =>
            await _personService.AddAsync(personSaveRequest);

        [HttpPut("update-person")]
        [CommandsResponseTypes]
        public async Task<bool> UpdateAsync([FromBody] PersonUpdateRequest personUpdateRequest) =>
            await _personService.UpdateAsync(personUpdateRequest);

        [HttpDelete("delete-person")]
        [CommandsResponseTypes]
        public async Task<bool> DeleteAsync([FromQuery] int id) =>
            await _personService.DeleteAsync(id);

        [HttpGet("find-person")]
        [QueryCommandsResponseTypes]
        public async Task<PersonResponse> FindByIdAsync([FromQuery] int id) =>
            await _personService.FindByIdAsync(id);

        [HttpGet("find-all-persons")]
        [QueryCommandsResponseTypes]
        public async Task<List<PersonResponse>> FindAllEntitiesAsync() =>
            await _personService.FindAllEntitiesAsync();

        [HttpGet("find-all-persons-paginated")]
        [QueryCommandsResponseTypes]
        public async Task<PageList<PersonResponse>> FindAllEntitiesWithPaginationAsync([FromQuery] PageParams pageParams) =>
            await _personService.FindAllEntitiesWithPaginationAsync(pageParams);
    }
}
