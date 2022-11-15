using ArquiteturaCamadas.Api.Controllers;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using Moq;

namespace UnitTests.ControllerTests
{
    public sealed class TagControllerTests
    {
        private readonly Mock<ITagService> _tagServiceMock;
        private readonly TagController _tagController;

        public TagControllerTests()
        {
            _tagServiceMock = new Mock<ITagService>();
            _tagController = new TagController(_tagServiceMock.Object);
        }
    }
}
