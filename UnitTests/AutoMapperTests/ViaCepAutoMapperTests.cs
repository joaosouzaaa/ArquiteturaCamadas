using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.ViaCep;
using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Domain.Entities;
using TestBuilders;

namespace UnitTests.AutoMapperTests
{
    public sealed class ViaCepAutoMapperTests
    {
        public ViaCepAutoMapperTests()
        {
            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public void ViaCepAddressResponse_To_Address()
        {
            var viaCepAddressResponse = ViaCepBuilder.NewObject().AddressResponseBuild();
            var address = viaCepAddressResponse.MapTo<ViaCepAddressResponse, Address>();

            Assert.Equal(address.ZipCode, viaCepAddressResponse.Cep.CleanCaracters());
            Assert.Equal(address.Complement, viaCepAddressResponse.Complemento);
            Assert.Equal(address.District, viaCepAddressResponse.Bairro);
            Assert.Equal(address.City, viaCepAddressResponse.Localidade);
            Assert.Equal(address.State, viaCepAddressResponse.Uf);
            Assert.Equal(address.Street, viaCepAddressResponse.Logradouro);
        }
    }
}
