using ArquiteturaCamadas.Business.Extensions;

namespace UnitTests.ExtensionsTests
{
    public sealed class AllowedFileFormatsExtensionTests
    {
        [Theory]
        [InlineData(".jpg")]
        [InlineData(".jpeg")]
        [InlineData(".png")]
        [InlineData(".jfif")]
        [InlineData(".pdf")]
        public void ValidateFileFormat_ReturnsTrue_FormatValid(string fileFormat)
        {
            var isValidFileFormat = fileFormat.ValidateFileFormat();

            Assert.True(isValidFileFormat); 
        }

        [Fact]
        public void ValidateFileFormat_ReturnsFalse_FormatInvalid()
        {
            var invalidFileFormat = ".joao";

            var isValidFileFormat = invalidFileFormat.ValidateFileFormat();

            Assert.False(isValidFileFormat);
        }
    }
}
