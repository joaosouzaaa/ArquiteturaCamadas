using ArquiteturaCamadas.Api.Controllers;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using Moq;
using TestBuilders;

namespace UnitTests.ControllerTests
{
    public sealed class PersonControllerTests
    {
        private readonly Mock<IPersonService> _personServiceMock;
        private readonly PersonController _personController;

        public PersonControllerTests()
        {
            _personServiceMock = new Mock<IPersonService>();
            _personController = new PersonController(_personServiceMock.Object);
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            _personServiceMock.Setup(ps => ps.AddAsync(personSaveRequest)).ReturnsAsync(true);

            var controllerResult = await _personController.AddAsync(personSaveRequest);

            _personServiceMock.Verify(ps => ps.AddAsync(personSaveRequest), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task AddAsync_ReturnsFalse()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            _personServiceMock.Setup(ps => ps.AddAsync(personSaveRequest)).ReturnsAsync(false);

            var controllerResult = await _personController.AddAsync(personSaveRequest);

            _personServiceMock.Verify(ps => ps.AddAsync(personSaveRequest), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            _personServiceMock.Setup(ps => ps.UpdateAsync(personUpdateRequest)).ReturnsAsync(true);

            var controllerResult = await _personController.UpdateAsync(personUpdateRequest);

            _personServiceMock.Verify(ps => ps.UpdateAsync(personUpdateRequest), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            _personServiceMock.Setup(ps => ps.UpdateAsync(personUpdateRequest)).ReturnsAsync(false);

            var controllerResult = await _personController.UpdateAsync(personUpdateRequest);

            _personServiceMock.Verify(ps => ps.UpdateAsync(personUpdateRequest), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            var id = 1;
            _personServiceMock.Setup(ps => ps.DeleteAsync(id)).ReturnsAsync(true);

            var controllerResult = await _personController.DeleteAsync(id);

            _personServiceMock.Verify(ps => ps.DeleteAsync(id), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            var id = 1;
            _personServiceMock.Setup(ps => ps.DeleteAsync(id)).ReturnsAsync(false);

            var controllerResult = await _personController.DeleteAsync(id);

            _personServiceMock.Verify(ps => ps.DeleteAsync(id), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            var id = 1;
            var personResponse = PersonBuilder.NewObject().ResponseBuild();
            _personServiceMock.Setup(ps => ps.FindByIdAsync(id)).ReturnsAsync(personResponse);

            var controllerResult = await _personController.FindByIdAsync(id);

            _personServiceMock.Verify(ps => ps.FindByIdAsync(id), Times.Once());
            Assert.Equal(personResponse, controllerResult);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            var personResponseList = new List<PersonResponse>()
            {
                PersonBuilder.NewObject().ResponseBuild(),
                PersonBuilder.NewObject().ResponseBuild()
            };
            _personServiceMock.Setup(ps => ps.FindAllEntitiesAsync()).ReturnsAsync(personResponseList);

            var controllerResult = await _personController.FindAllEntitiesAsync();

            _personServiceMock.Verify(ps => ps.FindAllEntitiesAsync(), Times.Once());
            Assert.Equal(controllerResult, personResponseList);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();
            var personResponseList = new List<PersonResponse>()
            {
                PersonBuilder.NewObject().ResponseBuild(),
                PersonBuilder.NewObject().ResponseBuild(),
                PersonBuilder.NewObject().ResponseBuild()
            };
            var personResponsePageList = new PageList<PersonResponse>(personResponseList, personResponseList.Count, pageParams);
            _personServiceMock.Setup(ps => ps.FindAllEntitiesWithPaginationAsync(pageParams)).ReturnsAsync(personResponsePageList);

            var controllerResult = await _personController.FindAllEntitiesWithPaginationAsync(pageParams);

            Assert.Equal(controllerResult, personResponsePageList);
        }
    }
}
