using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Address;
using ArquiteturaCamadas.Domain.Entities;
using TestBuilders;

namespace UnitTests.AutoMapperTests
{
    public sealed class AddressAutoMapperTests
    {
        public AddressAutoMapperTests()
        {
            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public void Address_To_AddressResponse()
        {
            var address = AddressBuilder.NewObject().DomainBuild();
            var addressResponse = address.MapTo<Address, AddressResponse>();

            Assert.Equal(address.Id, addressResponse.Id);
            Assert.Equal(address.ZipCode, addressResponse.ZipCode);
            Assert.Equal(address.Street, addressResponse.Street);
            Assert.Equal(address.City, addressResponse.City);
            Assert.Equal(address.State, addressResponse.State);
            Assert.Equal(address.District, addressResponse.District);
            Assert.Equal(address.Number, addressResponse.Number);
            Assert.Equal(address.Complement, addressResponse.Complement);
        }
    }
}
