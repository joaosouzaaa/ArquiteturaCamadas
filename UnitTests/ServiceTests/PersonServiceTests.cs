using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Services;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Business.Settings.NotificationSettings;
using ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation;
using ArquiteturaCamadas.Domain.Entities;
using FluentValidation;
using Moq;
using TestBuilders;

namespace UnitTests.ServiceTests
{
    public sealed class PersonServiceTests
    {
        private readonly Mock<IPersonRepository> _repositoryMock;
        private readonly Mock<IValidator<Person>> _validateMock;
        private readonly Mock<INotificationHandler> _notificationMock;
        private readonly Mock<ICepService> _cepServiceMock;
        private readonly PersonService _service;

        public PersonServiceTests()
        {
            _repositoryMock = new Mock<IPersonRepository>();
            _validateMock = new Mock<IValidator<Person>>();
            _cepServiceMock = new Mock<ICepService>();
            _notificationMock = new Mock<INotificationHandler>();
            _service = new PersonService(_repositoryMock.Object, _cepServiceMock.Object,
                                         _validateMock.Object, _notificationMock.Object);

            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Person>())).ReturnsAsync(true);

            var serviceResult = await _service.AddAsync(personSaveRequest);

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Once());
            Assert.True(serviceResult);
        }

        [Fact]
        public async Task AddAsync_EntityInvalid_ReturnsFalse()
        {
            var personSaveRequest = PersonBuilder.NewObject().WithName("a").SaveRequestBuild();

            var serviceResult = await _service.AddAsync(personSaveRequest);

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            _repositoryMock.Setup(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id)).ReturnsAsync(true);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Person>())).ReturnsAsync(true);

            var serviceResult = await _service.UpdateAsync(personUpdateRequest);

            _repositoryMock.Verify(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id), Times.Once());
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Once());
            Assert.True(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_EntityDoesNotExist_ReturnsFalse()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            _repositoryMock.Setup(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id)).ReturnsAsync(false);

            var serviceResult = await _service.UpdateAsync(personUpdateRequest);

            _repositoryMock.Verify(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id), Times.Once());
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_EntityInvalid_ReturnsFalse()
        {
            var personUpdateRequest = PersonBuilder.NewObject().WithName("aa").UpdateRequestBuild();
            _repositoryMock.Setup(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id)).ReturnsAsync(true);

            var serviceResult = await _service.UpdateAsync(personUpdateRequest);

            _repositoryMock.Verify(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id), Times.Once());
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            var id = 1;
            _repositoryMock.Setup(r => r.HaveObjectInDbAsync(r => r.Id == id)).ReturnsAsync(true);
            _repositoryMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var serviceResult = await _service.DeleteAsync(id);

            _repositoryMock.Verify(r => r.HaveObjectInDbAsync(r => r.Id == id), Times.Once());
            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once());
            Assert.True(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            var id = 1;
            _repositoryMock.Setup(r => r.HaveObjectInDbAsync(r => r.Id == id)).ReturnsAsync(false);

            var serviceResult = await _service.DeleteAsync(id);

            _repositoryMock.Verify(r => r.HaveObjectInDbAsync(r => r.Id == id), Times.Once());
            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            var id = 1;
            var person = PersonBuilder.NewObject().DomainBuild();
            _repositoryMock.Setup(r => r.FindByIdAsync(id, null, false)).ReturnsAsync(person);

            var serviceResult = await _service.FindByIdAsync(id);

            _repositoryMock.Verify(r => r.FindByIdAsync(id, null, false), Times.Once());
            Assert.NotNull(serviceResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsNull()
        {
            var id = 1;
            _repositoryMock.Setup(r => r.FindByIdAsync(id, null, false));

            var serviceResult = await _service.FindByIdAsync(id);

            _repositoryMock.Verify(r => r.FindByIdAsync(id, null, false), Times.Once());
            Assert.Null(serviceResult);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            var personList = new List<Person>()
            {
                PersonBuilder.NewObject().DomainBuild(),
                PersonBuilder.NewObject().DomainBuild()
            };
            _repositoryMock.Setup(r => r.FindAllEntitiesAsync(null)).ReturnsAsync(personList);

            var serviceResult = await _service.FindAllEntitiesAsync();

            _repositoryMock.Verify(r => r.FindAllEntitiesAsync(null), Times.Once());
            Assert.Equal(serviceResult.Count, personList.Count);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEmptyList()
        {
            _repositoryMock.Setup(r => r.FindAllEntitiesAsync(null));

            var serviceResult = await _service.FindAllEntitiesAsync();

            _repositoryMock.Verify(r => r.FindAllEntitiesAsync(null), Times.Once());
            Assert.Empty(serviceResult);
        }
    }
}
