using ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation;
using TestBuilders;

namespace UnitTests.ValidationTests
{
    public sealed class PersonValidationTests
    {
        private readonly PersonValidation _personValidation;

        public PersonValidationTests()
        {
            _personValidation = new PersonValidation();
        }

        [Fact]
        public async Task ValidatePerson_EntityValid_ReturnsTrue()
        {
            var person = PersonBuilder.NewObject().DomainBuild();

            var validationResult = await _personValidation.ValidateAsync(person);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineData("aa")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task ValidateAddress_NameInvalid_ReturnsFalse(string name)
        {
            var person = PersonBuilder.NewObject().WithName(name).DomainBuild();

            var validationResult = await _personValidation.ValidateAsync(person);

            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(0)]
        public async Task ValidatePerson_AgeInvalid_ReturnsFalse(int age)
        {
            var person = PersonBuilder.NewObject().WithAge(age).DomainBuild();

            var validationResult = await _personValidation.ValidateAsync(person);

            Assert.False(validationResult.IsValid);
        }
    }
}
