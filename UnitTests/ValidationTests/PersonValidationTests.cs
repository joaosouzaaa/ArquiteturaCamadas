using ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation;
using TestBuilders;

namespace UnitTests.ValidationTests
{
    public class PersonValidationTests
    {
        PersonValidation _validate;

        public PersonValidationTests()
        {
            _validate = new PersonValidation();
        }

        [Fact]
        public async Task ValidateEntity_EntityValid()
        {
            var person = PersonBuilder.NewObject().DomainBuild();

            var validationResult = await _validate.ValidateAsync(person);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineData("aa")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task ValidateEntity_NameInvalid(string name)
        {
            var person = PersonBuilder.NewObject().WithName(name).DomainBuild();

            var validationResult = await _validate.ValidateAsync(person);

            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(17)]
        [InlineData(0)]
        public async Task ValidateEntity_AgeInvalid(int age)
        {
            var person = PersonBuilder.NewObject().WithAge(age).DomainBuild();

            var validationResult = await _validate.ValidateAsync(person);

            Assert.False(validationResult.IsValid);
        }
    }
}
