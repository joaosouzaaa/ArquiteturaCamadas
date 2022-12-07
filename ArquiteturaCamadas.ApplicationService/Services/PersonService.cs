using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Services.ServiceBase;
using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ArquiteturaCamadas.ApplicationService.Services
{
    public sealed class PersonService : BaseService<Person>, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICepService _cepService;

        public PersonService(IPersonRepository personRepository, ICepService cepService,
                             IValidator<Person> validator, INotificationHandler notification) 
                             : base(validator, notification)
        {
            _personRepository = personRepository;
            _cepService = cepService;
        }

        public async Task<bool> AddAsync(PersonSaveRequest personSaveRequest)
        {
            var address = await _cepService.GetAddressFromCepAsync(personSaveRequest.Address.ZipCode);

            if (address is null)
                return false;

            address.Number = personSaveRequest.Address.Number;

            if (address.Complement is not null)
                address.Complement = personSaveRequest.Address.Complement;

            var person = personSaveRequest.MapTo<PersonSaveRequest, Person>();
            person.Address = address;

            if (!await ValidateAsync(person))
                return false;

            return await _personRepository.AddAsync(person);
        }

        public async Task<bool> UpdateAsync(PersonUpdateRequest personUpdateRequest)
        {
            var person = await _personRepository.FindByIdAsync(personUpdateRequest.Id, p => p.Include(p => p.Address), true);
            
            if (person is null)
                return _notification.AddDomainNotification(EMessage.NotFound.ToString(), EMessage.NotFound.Description().FormatTo("Person"));

            var address = await _cepService.GetAddressFromCepAsync(personUpdateRequest.Address.ZipCode);

            if (address is null)
                return false;

            address.Id = person.Address.Id;
            address.Number = personUpdateRequest.Address.Number;

            if (address.Complement is not null)
                address.Complement = personUpdateRequest.Address.Complement;

            person = personUpdateRequest.MapTo<PersonUpdateRequest, Person>();
            person.Address = address;

            if (!await ValidateAsync(person))
                return false;

            return await _personRepository.UpdateAsync(person);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _personRepository.HaveObjectInDbAsync(p => p.Id == id))
                return _notification.AddDomainNotification(EMessage.NotFound.ToString(), EMessage.NotFound.Description().FormatTo("Person"));

            return await _personRepository.DeleteAsync(id);
        }

        public async Task<PersonResponse> FindByIdAsync(int id)
        {
            var person = await _personRepository.FindByIdAsync(id, p => p.Include(p => p.Address));

            return person.MapTo<Person, PersonResponse>();
        }

        public async Task<List<PersonResponse>> FindAllEntitiesAsync()
        {
            var personsList = await _personRepository.FindAllEntitiesAsync(p => p.Include(p => p.Address));

            return personsList.MapTo<List<Person>, List<PersonResponse>>();
        }

        public async Task<PageList<PersonResponse>> FindAllEntitiesWithPaginationAsync(PageParams pageParams)
        {
            var personPageList = await _personRepository.FindAllEntitiesWithPaginationAsync(pageParams, p => p.Include(p => p.Address));

            return personPageList.MapTo<PageList<Person>, PageList<PersonResponse>>();
        }
    }
}
