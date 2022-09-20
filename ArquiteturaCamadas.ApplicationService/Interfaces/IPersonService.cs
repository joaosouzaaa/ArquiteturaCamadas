using ArquiteturaCamadas.ApplicationService.Interfaces.ServiceBase;
using ArquiteturaCamadas.ApplicationService.Requests.Person;
using ArquiteturaCamadas.ApplicationService.Responses;

namespace ArquiteturaCamadas.ApplicationService.Interfaces
{
    public interface IPersonService : 
                     ICommandsService<PersonSaveRequest, PersonUpdateRequest>, 
                     IQueryCommandsService<PersonResponse>
    {
    }
}
