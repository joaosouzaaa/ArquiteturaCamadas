using ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation;
using TestBuilders;

namespace UnitTests.ValidationTests
{
    public sealed class AddressValidationTests
    {
        private readonly AddressValidation _addressValidation;

        public AddressValidationTests()
        {
            _addressValidation = new AddressValidation();
        }

        [Fact]
        public async Task ValidateAddress_EntityValid_ReturnTrue()
        {
            var address = AddressBuilder.NewObject().DomainBuild();

            var validationResult = await _addressValidation.ValidateAsync(address);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineData("2929181")]
        [InlineData("-1")]
        [InlineData("")]
        [InlineData("joao")]
        public async Task ValidateAddress_ZipCodeInvalid_ReturnsFalse(string zipCode)
        {
            var address = AddressBuilder.NewObject().WithZipCode(zipCode).DomainBuild();

            var validationResult = await _addressValidation.ValidateAsync(address);

            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [InlineData("j")]
        [InlineData("")]
        [InlineData("jaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task ValidateAddres_CityInvalid_ReturnsFalse(string city)
        {
            var address = AddressBuilder.NewObject().WithCity(city).DomainBuild();

            var validationResult = await _addressValidation.ValidateAsync(address);

            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [InlineData("j")]
        [InlineData("")]
        [InlineData("jaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task ValidateAddress_StreetInvalid_ReturnsFalse(string street)
        {
            var address = AddressBuilder.NewObject().WithStreet(street).DomainBuild();

            var validationResult = await _addressValidation.ValidateAsync(address);

            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [InlineData("j")]
        [InlineData("jaaaa")]
        [InlineData("")]
        public async Task ValidateAddress_StateInvalid_ReturnsFalse(string state)
        {
            var address = AddressBuilder.NewObject().WithState(state).DomainBuild();

            var validationResult = await _addressValidation.ValidateAsync(address);

            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("jaaaaaaaaaaaaaaaa")]
        public async Task ValidateAddress_NumberInvalid_ReturnsFalse(string number)
        {
            var address = AddressBuilder.NewObject().WithNumber(number).DomainBuild();

            var validationResult = await _addressValidation.ValidateAsync(address);

            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [InlineData("j")]
        [InlineData("")]
        [InlineData("jaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task ValidateAddress_DistrictInvalid_ReturnsFalse(string district)
        {
            var address = AddressBuilder.NewObject().WithDistrict(district).DomainBuild();

            var validationResult = await _addressValidation.ValidateAsync(address);

            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [InlineData("j")]
        [InlineData("")]
        [InlineData("jaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaajaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task ValidateAddress_ComplementInvalid_ReturnsFalse(string complement)
        {
            var address = AddressBuilder.NewObject().WithComplement(complement).DomainBuild();

            var validationResult = await _addressValidation.ValidateAsync(address);

            Assert.False(validationResult.IsValid);
        }
    }
}
