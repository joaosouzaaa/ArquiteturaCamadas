using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Address;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Address;
using ArquiteturaCamadas.Domain.Entities;
using AutoMapper;

namespace ArquiteturaCamadas.ApplicationService.AutoMapperSettings.Profiles
{
    public sealed class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressResponse>();
        }
    }
}
