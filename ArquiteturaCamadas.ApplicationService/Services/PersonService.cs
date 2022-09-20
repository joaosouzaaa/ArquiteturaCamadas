using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Requests.Person;
using ArquiteturaCamadas.ApplicationService.Responses;
using ArquiteturaCamadas.ApplicationService.Services.ServiceBase;
using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Business.Interfaces.Validation;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;

namespace ArquiteturaCamadas.ApplicationService.Services
{
    public class PersonService : BaseService<Person>, IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository, IValidate<Person> validator, INotificationHandler notification) 
                             : base(validator, notification)
        {
            _personRepository = personRepository;
        }

        public async Task<bool> AddAsync(PersonSaveRequest save)
        {
            var person = save.MapTo<PersonSaveRequest, Person>();

            if (!await ValidateAsync(person))
                return false;

            return await _personRepository.AddAsync(person);
        }

        public async Task<bool> UpdateAsync(PersonUpdateRequest update)
        {
            if (!await _personRepository.HaveObjectInDbAsync(p => p.Id == update.Id))
                return _notification.AddDomainNotification("Not Found", EMessage.NotFound.Description().FormatTo("Person"));

            var person = update.MapTo<PersonUpdateRequest, Person>();

            if (!await ValidateAsync(person))
                return false;

            return await _personRepository.UpdateAsync(person);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _personRepository.HaveObjectInDbAsync(p => p.Id == id))
                return _notification.AddDomainNotification("Not Found", EMessage.NotFound.Description().FormatTo("Person"));

            return await _personRepository.DeleteAsync(id);
        }

        public async Task<PersonResponse> FindByIdAsync(int id)
        {
            var person = await _personRepository.FindByIdAsync(id);

            return person.MapTo<Person, PersonResponse>();
        }

        public async Task<List<PersonResponse>> FindAllEntitiesAsync()
        {
            var personsList = await _personRepository.FindAllEntitiesAsync();

            return personsList.MapTo<List<Person>, List<PersonResponse>>();
        }
    }
}
