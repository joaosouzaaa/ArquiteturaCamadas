using ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation;
using TestBuilders;

namespace UnitTests.ValidationTests
{
    public sealed class PostValidationTests
    {
        private readonly PostValidation _postValidation;

        public PostValidationTests()
        {
            _postValidation = new PostValidation();
        }

        [Fact]
        public async Task ValidatePost_EntityValid_ReturnsTrue()
        {
            var post = PostBuilder.NewObject().DomainBuild();

            var validationResult = await _postValidation.ValidateAsync(post);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public async Task ValidatePost_MessageInvalid_MessageTooLarge_ReturnsFalse()
        {
            var message = new string('a', 640);
            var post = PostBuilder.NewObject().WithMessage(message).DomainBuild();

            var validationResult = await _postValidation.ValidateAsync(post);

            Assert.False(validationResult.IsValid);
        }
    }
}
