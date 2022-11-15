using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.ViaCep;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Domain.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace ArquiteturaCamadas.ApplicationService.Services
{
    public sealed class CepService : ICepService
    {
        private readonly INotificationHandler _notification;
        private readonly IHttpClientFactory _httpClientFactory;

        public CepService(INotificationHandler notification, IHttpClientFactory httpClientFactory)
        {
            _notification = notification;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Address> GetAddressFromCepAsync(string cep)
        {
            if(cep.Length != 8)
            {
                _notification.AddDomainNotification("Cep", "Tamanho de cep incorreto");
                return null;
            }

            var httpClient = _httpClientFactory.CreateClient("ViaCepHttpClient");

            var viaCepAddressResponse = await httpClient.GetFromJsonAsync<ViaCepAddressResponse>($"{cep}/json/");

            if(viaCepAddressResponse.Cep is null)
            {
                _notification.AddDomainNotification("Cep", "Cep inválido");
                return null;
            }

            return viaCepAddressResponse.MapTo<ViaCepAddressResponse, Address>();
        }
    }
}
