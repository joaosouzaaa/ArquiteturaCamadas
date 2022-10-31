using ArquiteturaCamadas.Api.Controllers;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using Moq;
using TestBuilders;

namespace UnitTests.ControllerTests
{
    public class PersonControllerTests
    {
        Mock<IPersonService> _service;
        PersonController _controller;

        public PersonControllerTests()
        {
            _service = new Mock<IPersonService>();
            _controller = new PersonController(_service.Object);
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            _service.Setup(s => s.AddAsync(personSaveRequest)).ReturnsAsync(true);

            var controllerResult = await _controller.AddAsync(personSaveRequest);

            _service.Verify(s => s.AddAsync(personSaveRequest), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task AddAsync_ReturnsFalse()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            _service.Setup(s => s.AddAsync(personSaveRequest)).ReturnsAsync(false);

            var controllerResult = await _controller.AddAsync(personSaveRequest);

            _service.Verify(s => s.AddAsync(personSaveRequest), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            _service.Setup(s => s.UpdateAsync(personUpdateRequest)).ReturnsAsync(true);

            var controllerResult = await _controller.UpdateAsync(personUpdateRequest);

            _service.Verify(s => s.UpdateAsync(personUpdateRequest), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            _service.Setup(s => s.UpdateAsync(personUpdateRequest)).ReturnsAsync(false);

            var controllerResult = await _controller.UpdateAsync(personUpdateRequest);

            _service.Verify(s => s.UpdateAsync(personUpdateRequest), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            var id = 1;
            _service.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

            var controllerResult = await _controller.DeleteAsync(id);

            _service.Verify(s => s.DeleteAsync(id), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            var id = 1;
            _service.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

            var controllerResult = await _controller.DeleteAsync(id);

            _service.Verify(s => s.DeleteAsync(id), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            var id = 1;
            var personResponse = PersonBuilder.NewObject().ResponseBuild();
            _service.Setup(s => s.FindByIdAsync(id)).ReturnsAsync(personResponse);

            var controllerResult = await _controller.FindByIdAsync(id);

            _service.Verify(s => s.FindByIdAsync(id), Times.Once());
            Assert.Equal(personResponse, controllerResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsNull()
        {
            var id = 1;
            _service.Setup(s => s.FindByIdAsync(id));

            var controllerResult = await _controller.FindByIdAsync(id);

            _service.Verify(s => s.FindByIdAsync(id), Times.Once());
            Assert.Null(controllerResult);
        }

        [Fact]
        public async Task FindAllAsync_ReturnsEntities()
        {
            var personResponseList = new List<PersonResponse>()
            {
                PersonBuilder.NewObject().ResponseBuild(),
                PersonBuilder.NewObject().ResponseBuild()
            };
            _service.Setup(s => s.FindAllEntitiesAsync()).ReturnsAsync(personResponseList);

            var controllerResult = await _controller.FindAllEntitiesAsync();

            _service.Verify(s => s.FindAllEntitiesAsync(), Times.Once());
            Assert.Equal(controllerResult, personResponseList);
        }

        [Fact]
        public async Task FindAllAsync_ReturnsEmptyList()
        {
            _service.Setup(s => s.FindAllEntitiesAsync());

            var controllerResult = await _controller.FindAllEntitiesAsync();

            _service.Verify(s => s.FindAllEntitiesAsync(), Times.Once());
            Assert.Null(controllerResult);
        }
    }
}
