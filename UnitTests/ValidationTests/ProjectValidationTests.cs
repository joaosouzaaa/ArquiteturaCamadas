using ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation;
using TestBuilders;

namespace UnitTests.ValidationTests
{
    public sealed class ProjectValidationTests
    {
        private readonly ProjectValidation _projectValidation;

        public ProjectValidationTests()
        {
            _projectValidation= new ProjectValidation();
        }

        [Fact]
        public async Task ValidateProjectAsync_EntityValid_ReturnsTrue()
        {
            var project = ProjectBuilder.NewObject().DomainBuild();

            var validationResult = await _projectValidation.ValidateAsync(project);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task ValidateProjectAsync_NameInvalid_ReturnsFalse(string name)
        {
            var project = ProjectBuilder.NewObject().WithName(name).DomainBuild();

            var validationResult = await _projectValidation.ValidateAsync(project);

            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [InlineData(-45)]
        [InlineData(0)]
        public async Task ValidateProjectAsync_ValueInvalid_ReturnsFalse(decimal value)
        {
            var project = ProjectBuilder.NewObject().WithValue(value).DomainBuild();

            var validationResult = await _projectValidation.ValidateAsync(project);

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task ValidateProjectAsync_ExpiryDateInvalid_ReturnsFalse()
        {
            var invalidExpiryDate = DateTime.Now.AddDays(-2);
            var project = ProjectBuilder.NewObject().WithExpiryDate(invalidExpiryDate).DomainBuild();

            var validationResult = await _projectValidation.ValidateAsync(project);

            Assert.False(validationResult.IsValid);
        }
    }
}
