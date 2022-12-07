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
            // A
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();

            _personServiceMock.Setup(ps => ps.AddAsync(personSaveRequest)).ReturnsAsync(true);

            // A
            var controllerResult = await _personController.AddAsync(personSaveRequest);

            // A
            _personServiceMock.Verify(ps => ps.AddAsync(personSaveRequest), Times.Once());

            Assert.True(controllerResult);
        }

        [Fact]
        public async Task AddAsync_ReturnsFalse()
        {
            // A
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();

            _personServiceMock.Setup(ps => ps.AddAsync(personSaveRequest)).ReturnsAsync(false);

            // A
            var controllerResult = await _personController.AddAsync(personSaveRequest);

            // A
            _personServiceMock.Verify(ps => ps.AddAsync(personSaveRequest), Times.Once());

            Assert.False(controllerResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            // A
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();

            _personServiceMock.Setup(ps => ps.UpdateAsync(personUpdateRequest)).ReturnsAsync(true);

            // A
            var controllerResult = await _personController.UpdateAsync(personUpdateRequest);

            // A
            _personServiceMock.Verify(ps => ps.UpdateAsync(personUpdateRequest), Times.Once());

            Assert.True(controllerResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse()
        {
            // A
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();

            _personServiceMock.Setup(ps => ps.UpdateAsync(personUpdateRequest)).ReturnsAsync(false);

            // A
            var controllerResult = await _personController.UpdateAsync(personUpdateRequest);

            // A
            _personServiceMock.Verify(ps => ps.UpdateAsync(personUpdateRequest), Times.Once());

            Assert.False(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // A
            var id = 1;

            _personServiceMock.Setup(ps => ps.DeleteAsync(id)).ReturnsAsync(true);

            // A
            var controllerResult = await _personController.DeleteAsync(id);

            // A
            _personServiceMock.Verify(ps => ps.DeleteAsync(id), Times.Once());

            Assert.True(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            // A
            var id = 1;

            _personServiceMock.Setup(ps => ps.DeleteAsync(id)).ReturnsAsync(false);

            // A
            var controllerResult = await _personController.DeleteAsync(id);

            // A
            _personServiceMock.Verify(ps => ps.DeleteAsync(id), Times.Once());

            Assert.False(controllerResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            // A
            var id = 1;
            var personResponse = PersonBuilder.NewObject().ResponseBuild();

            _personServiceMock.Setup(ps => ps.FindByIdAsync(id)).ReturnsAsync(personResponse);

            // A
            var controllerResult = await _personController.FindByIdAsync(id);

            // A
            _personServiceMock.Verify(ps => ps.FindByIdAsync(id), Times.Once());

            Assert.Equal(personResponse, controllerResult);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            // A
            var personResponseList = new List<PersonResponse>()
            {
                PersonBuilder.NewObject().ResponseBuild(),
                PersonBuilder.NewObject().ResponseBuild()
            };

            _personServiceMock.Setup(ps => ps.FindAllEntitiesAsync()).ReturnsAsync(personResponseList);

            // A
            var controllerResult = await _personController.FindAllEntitiesAsync();

            // A
            _personServiceMock.Verify(ps => ps.FindAllEntitiesAsync(), Times.Once());

            Assert.Equal(controllerResult, personResponseList);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            // A
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();
            var personResponseList = new List<PersonResponse>()
            {
                PersonBuilder.NewObject().ResponseBuild(),
                PersonBuilder.NewObject().ResponseBuild(),
                PersonBuilder.NewObject().ResponseBuild()
            };
            var personResponsePageList = new PageList<PersonResponse>(personResponseList, personResponseList.Count, pageParams);

            _personServiceMock.Setup(ps => ps.FindAllEntitiesWithPaginationAsync(pageParams)).ReturnsAsync(personResponsePageList);

            // A
            var controllerResult = await _personController.FindAllEntitiesWithPaginationAsync(pageParams);

            // A
            _personServiceMock.Verify(ps => ps.FindAllEntitiesWithPaginationAsync(pageParams), Times.Once());

            Assert.Equal(controllerResult, personResponsePageList);
        }
    }
}
