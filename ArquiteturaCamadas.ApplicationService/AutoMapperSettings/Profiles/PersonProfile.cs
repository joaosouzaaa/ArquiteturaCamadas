using ArquiteturaCamadas.ApplicationService.Requests.Person;
using ArquiteturaCamadas.ApplicationService.Responses;
using ArquiteturaCamadas.Domain.Entities;
using AutoMapper;

namespace ArquiteturaCamadas.ApplicationService.AutoMapperSettings.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonSaveRequest>()
                .ForMember(ps => ps.Gender, map => map.MapFrom(p => p.Gender))
                .ReverseMap();

            CreateMap<Person, PersonUpdateRequest>()
                .ForMember(pu => pu.Gender, map => map.MapFrom(p => p.Gender))
                .ReverseMap();

            CreateMap<Person, PersonResponse>()
                .ReverseMap();
        }
    }
}
