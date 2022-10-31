using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using AutoMapper;

namespace ArquiteturaCamadas.ApplicationService.AutoMapperSettings.Profiles
{
    public sealed class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<PersonSaveRequest, Person>();

            CreateMap<PersonUpdateRequest, Person>();

            CreateMap<Person, PersonResponse>();

            CreateMap<PageList<Person>, PageList<PersonResponse>>();
        }
    }
}
