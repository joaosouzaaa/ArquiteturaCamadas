using ArquiteturaCamadas.Api.ControllersAttributes;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Requests.Person;
using ArquiteturaCamadas.ApplicationService.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArquiteturaCamadas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost("add_person")]
        [CommandsResponseTypes]
        public async Task<bool> AddAsync(PersonSaveRequest personSaveRequest) =>
            await _personService.AddAsync(personSaveRequest);

        [HttpPut("update_person")]
        [CommandsResponseTypes]
        public async Task<bool> UpdateAsync(PersonUpdateRequest personUpdateRequest) =>
            await _personService.UpdateAsync(personUpdateRequest);

        [HttpDelete("delete_person")]
        [CommandsResponseTypes]
        public async Task<bool> DeleteAsync(int id) =>
            await _personService.DeleteAsync(id);

        [HttpGet("find_person")]
        [QueryCommandsResponseTypes]
        public async Task<PersonResponse> FindByIdAsync(int id) =>
            await _personService.FindByIdAsync(id);

        [HttpGet("find_all_persons")]
        [QueryCommandsResponseTypes]
        public async Task<List<PersonResponse>> FindAllEntitiesAsync() =>
            await _personService.FindAllEntitiesAsync();
    }
}
