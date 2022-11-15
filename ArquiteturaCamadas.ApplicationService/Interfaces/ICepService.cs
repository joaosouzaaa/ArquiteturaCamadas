using ArquiteturaCamadas.Domain.Entities;

namespace ArquiteturaCamadas.ApplicationService.Interfaces
{
    public interface ICepService
    {
        Task<Address> GetAddressFromCepAsync(string cep);
    }
}
