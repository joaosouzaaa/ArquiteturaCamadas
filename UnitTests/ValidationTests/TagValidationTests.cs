using ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation;
using TestBuilders;

namespace UnitTests.ValidationTests
{
    public sealed class TagValidationTests
    {
        private readonly TagValidation _tagValidation;

        public TagValidationTests()
        {
            _tagValidation = new TagValidation();
        }

        [Fact]
        public async Task ValidateTag_EntityValid_ReturnsTrue()
        {
            var tag = TagBuilder.NewObject().DomainBuild();

            var validationResult = await _tagValidation.ValidateAsync(tag);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task ValidateTag_TagNameInvalid_ReturnsFalse(string tagName)
        {
            var tag = TagBuilder.NewObject().WithTagName(tagName).DomainBuild();

            var validationResult = await _tagValidation.ValidateAsync(tag);

            Assert.False(validationResult.IsValid);
        }
    }
}
