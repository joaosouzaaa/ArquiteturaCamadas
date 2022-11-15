using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.ViaCep;
using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Domain.Entities;
using AutoMapper;

namespace ArquiteturaCamadas.ApplicationService.AutoMapperSettings.Profiles
{
    public sealed class ViaCepProfile : Profile
    {
        public ViaCepProfile()
        {
            CreateMap<ViaCepAddressResponse, Address>()
                .ForMember(a => a.ZipCode, map => map.MapFrom(v => v.Cep.CleanCaracters()))
                .ForMember(a => a.Complement, map => map.MapFrom(v => v.Complemento))
                .ForMember(a => a.District, map => map.MapFrom(v => v.Bairro))
                .ForMember(a => a.City, map => map.MapFrom(v => v.Localidade))
                .ForMember(a => a.State, map => map.MapFrom(v => v.Uf))
                .ForMember(a => a.Street, map => map.MapFrom(v => v.Logradouro));
        }
    }
}
