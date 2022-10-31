using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Domain.Enums;

namespace UnitTests.ExtensionsTests
{
    public sealed class MessageExtensionTests
    {
        [Fact]
        public void Description_Equals_AsIntended()
        {
            var messageDescription = EMessage.Required.Description();

            Assert.Equal(messageDescription, "{0} need to be filled");
        }
    }
}
