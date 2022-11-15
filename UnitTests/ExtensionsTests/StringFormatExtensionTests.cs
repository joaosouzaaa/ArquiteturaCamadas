using ArquiteturaCamadas.Business.Extensions;

namespace UnitTests.ExtensionsTests
{
    public sealed class StringFormatExtensionTests
    {
        [Fact]
        public void FormatTo_ReturnsFormatedString()
        {
            var formatedString = "{0} meu nome é {1}".FormatTo("oi", "joao");

            Assert.Equal("oi meu nome é joao", formatedString);
        }

        [Fact]
        public void CleanCaracters_CleanAllCaracters()
        {
            var valueToClean = "joaoasdsjdiasdi999".CleanCaracters();
            
            Assert.Equal(valueToClean, "999");
        }
    }
}
