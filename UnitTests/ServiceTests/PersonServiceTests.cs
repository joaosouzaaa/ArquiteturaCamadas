using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.Services;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Business.Settings.NotificationSettings;
using ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation;
using ArquiteturaCamadas.Domain.Entities;
using Moq;
using TestBuilders;

namespace UnitTests.ServiceTests
{
    public sealed class PersonServiceTests
    {
        Mock<IPersonRepository> _repository;
        PersonValidation _validate;
        NotificationHandler _notification;
        PersonService _service;

        public PersonServiceTests()
        {
            _repository = new Mock<IPersonRepository>();
            _validate = new PersonValidation();
            _notification = new NotificationHandler();
            _service = new PersonService(_repository.Object, _validate, _notification);

            AutoMapperConfigurations.Inicialize();
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            _repository.Setup(r => r.AddAsync(It.IsAny<Person>())).ReturnsAsync(true);

            var serviceResult = await _service.AddAsync(personSaveRequest);

            _repository.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Once());
            Assert.True(serviceResult);
        }

        [Fact]
        public async Task AddAsync_EntityInvalid_ReturnsFalse()
        {
            var personSaveRequest = PersonBuilder.NewObject().WithName("a").SaveRequestBuild();

            var serviceResult = await _service.AddAsync(personSaveRequest);

            _repository.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            _repository.Setup(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id)).ReturnsAsync(true);
            _repository.Setup(r => r.UpdateAsync(It.IsAny<Person>())).ReturnsAsync(true);

            var serviceResult = await _service.UpdateAsync(personUpdateRequest);

            _repository.Verify(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id), Times.Once());
            _repository.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Once());
            Assert.True(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_EntityDoesNotExist_ReturnsFalse()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            _repository.Setup(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id)).ReturnsAsync(false);

            var serviceResult = await _service.UpdateAsync(personUpdateRequest);

            _repository.Verify(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id), Times.Once());
            _repository.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_EntityInvalid_ReturnsFalse()
        {
            var personUpdateRequest = PersonBuilder.NewObject().WithName("aa").UpdateRequestBuild();
            _repository.Setup(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id)).ReturnsAsync(true);

            var serviceResult = await _service.UpdateAsync(personUpdateRequest);

            _repository.Verify(r => r.HaveObjectInDbAsync(r => r.Id == personUpdateRequest.Id), Times.Once());
            _repository.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            var id = 1;
            _repository.Setup(r => r.HaveObjectInDbAsync(r => r.Id == id)).ReturnsAsync(true);
            _repository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var serviceResult = await _service.DeleteAsync(id);

            _repository.Verify(r => r.HaveObjectInDbAsync(r => r.Id == id), Times.Once());
            _repository.Verify(r => r.DeleteAsync(id), Times.Once());
            Assert.True(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            var id = 1;
            _repository.Setup(r => r.HaveObjectInDbAsync(r => r.Id == id)).ReturnsAsync(false);

            var serviceResult = await _service.DeleteAsync(id);

            _repository.Verify(r => r.HaveObjectInDbAsync(r => r.Id == id), Times.Once());
            _repository.Verify(r => r.DeleteAsync(id), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            var id = 1;
            var person = PersonBuilder.NewObject().DomainBuild();
            _repository.Setup(r => r.FindByIdAsync(id, null, false)).ReturnsAsync(person);

            var serviceResult = await _service.FindByIdAsync(id);

            _repository.Verify(r => r.FindByIdAsync(id, null, false), Times.Once());
            Assert.NotNull(serviceResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsNull()
        {
            var id = 1;
            _repository.Setup(r => r.FindByIdAsync(id, null, false));

            var serviceResult = await _service.FindByIdAsync(id);

            _repository.Verify(r => r.FindByIdAsync(id, null, false), Times.Once());
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
            _repository.Setup(r => r.FindAllEntitiesAsync(null)).ReturnsAsync(personList);

            var serviceResult = await _service.FindAllEntitiesAsync();

            _repository.Verify(r => r.FindAllEntitiesAsync(null), Times.Once());
            Assert.Equal(serviceResult.Count, personList.Count);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEmptyList()
        {
            _repository.Setup(r => r.FindAllEntitiesAsync(null));

            var serviceResult = await _service.FindAllEntitiesAsync();

            _repository.Verify(r => r.FindAllEntitiesAsync(null), Times.Once());
            Assert.Empty(serviceResult);
        }
    }
}
