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
            CreateMap<PersonSaveRequest, Person>()
                .ForMember(p => p.Address, map => map.Ignore());

            CreateMap<PersonUpdateRequest, Person>()
                .ForMember(p => p.Address, map => map.Ignore());

            CreateMap<Person, PersonResponse>()
                .ForMember(pr => pr.Address, map => map.MapFrom(p => p.Address));

            CreateMap<PageList<Person>, PageList<PersonResponse>>();
        }
    }
}
