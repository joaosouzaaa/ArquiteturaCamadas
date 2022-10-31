using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person;
using ArquiteturaCamadas.ApplicationService.Interfaces.ServiceBase;

namespace ArquiteturaCamadas.ApplicationService.Interfaces
{
    public interface IPersonService : 
                     ICommandsService<PersonSaveRequest, PersonUpdateRequest>, 
                     IQueryCommandsService<PersonResponse>
    {
    }
}
