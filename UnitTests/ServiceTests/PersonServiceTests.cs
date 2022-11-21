using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Services;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Business.Settings.NotificationSettings;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation;
using ArquiteturaCamadas.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Net;
using TestBuilders;
using TestBuilders.Helpers;

namespace UnitTests.ServiceTests
{
    public sealed class PersonServiceTests
    {
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly Mock<IValidator<Person>> _personValidatorMock;
        private readonly Mock<INotificationHandler> _notificationMock;
        private readonly Mock<ICepService> _cepServiceMock;
        private readonly PersonService _personService;

        public PersonServiceTests()
        {
            _personRepositoryMock = new Mock<IPersonRepository>();
            _personValidatorMock = new Mock<IValidator<Person>>();
            _cepServiceMock = new Mock<ICepService>();
            _notificationMock = new Mock<INotificationHandler>();
            _personService = new PersonService(_personRepositoryMock.Object, _cepServiceMock.Object,
                                               _personValidatorMock.Object, _notificationMock.Object);

            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            var address = AddressBuilder.NewObject().DomainBuild();
            var validationResult = new ValidationResult();
            _cepServiceMock.Setup(cs => cs.GetAddressFromCepAsync(personSaveRequest.Address.ZipCode)).ReturnsAsync(address);
            _personValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Person>(), default)).ReturnsAsync(validationResult);
            _personRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Person>())).ReturnsAsync(true);

            var serviceResult = await _personService.AddAsync(personSaveRequest);

            _cepServiceMock.Verify(cs => cs.GetAddressFromCepAsync(personSaveRequest.Address.ZipCode), Times.Once());
            _personValidatorMock.Verify(pv => pv.ValidateAsync(It.IsAny<Person>(), default), Times.Once());
            _notificationMock.Verify(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            _personRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Once());
            Assert.True(serviceResult);
        }

        [Fact]
        public async Task AddAsync_AddressDoesNotExist_ReturnsFalse()
        {
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            _cepServiceMock.Setup(cs => cs.GetAddressFromCepAsync(personSaveRequest.Address.ZipCode));

            var serviceResult = await _personService.AddAsync(personSaveRequest);

            _cepServiceMock.Verify(cs => cs.GetAddressFromCepAsync(personSaveRequest.Address.ZipCode), Times.Once());
            _personValidatorMock.Verify(pv => pv.ValidateAsync(It.IsAny<Person>(), default), Times.Never());
            _notificationMock.Verify(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            _personRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task AddAsync_EntityInvalid_ReturnsFalse()
        {
            var personSaveRequest = PersonBuilder.NewObject().WithName("a").SaveRequestBuild();
            var address = AddressBuilder.NewObject().DomainBuild();
            var validationFailureList = new List<ValidationFailure>()
            {
                new ValidationFailure(){ErrorMessage = "random error"}
            };
            var validationResult = new ValidationResult()
            {
                Errors = validationFailureList
            };
            var validationFailureListCount = validationFailureList.Count;
            _cepServiceMock.Setup(cs => cs.GetAddressFromCepAsync(personSaveRequest.Address.ZipCode)).ReturnsAsync(address);
            _personValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Person>(), default)).ReturnsAsync(validationResult);
            AddDomainNotificationsInRange(validationFailureListCount);

            var serviceResult = await _personService.AddAsync(personSaveRequest);

            _cepServiceMock.Verify(cs => cs.GetAddressFromCepAsync(personSaveRequest.Address.ZipCode), Times.Once());
            _personValidatorMock.Verify(pv => pv.ValidateAsync(It.IsAny<Person>(), default), Times.Once());
            VerifyNotificationHandlerInRange(validationFailureListCount);
            _personRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            var address = AddressBuilder.NewObject().DomainBuild();
            var validationResult = new ValidationResult();
            var person = PersonBuilder.NewObject().DomainBuild();
            _personRepositoryMock.Setup(r => r.FindByIdAsync(personUpdateRequest.Id, UtilTools.MockIIncludableQuery<Person>(), true)).ReturnsAsync(person);
            _cepServiceMock.Setup(cs => cs.GetAddressFromCepAsync(personUpdateRequest.Address.ZipCode)).ReturnsAsync(address);
            _personValidatorMock.Setup(ps => ps.ValidateAsync(It.IsAny<Person>(), default)).ReturnsAsync(validationResult);
            _personRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Person>())).ReturnsAsync(true);

            var serviceResult = await _personService.UpdateAsync(personUpdateRequest);

            _personRepositoryMock.Verify(r => r.FindByIdAsync(personUpdateRequest.Id, UtilTools.MockIIncludableQuery<Person>(), true), Times.Once());
            _cepServiceMock.Verify(cs => cs.GetAddressFromCepAsync(personUpdateRequest.Address.ZipCode), Times.Once());
            _personValidatorMock.Verify(ps => ps.ValidateAsync(It.IsAny<Person>(), default), Times.Once());
            _notificationMock.Verify(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            _personRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Once());
            Assert.True(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_EntityDoesNotExist_ReturnsFalse()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            _personRepositoryMock.Setup(r => r.FindByIdAsync(personUpdateRequest.Id, UtilTools.MockIIncludableQuery<Person>(), true));
            
            var serviceResult = await _personService.UpdateAsync(personUpdateRequest);

            _personRepositoryMock.Verify(r => r.FindByIdAsync(personUpdateRequest.Id, UtilTools.MockIIncludableQuery<Person>(), true), Times.Once());
            _cepServiceMock.Verify(cs => cs.GetAddressFromCepAsync(It.IsAny<string>()), Times.Never());
            _notificationMock.Verify(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            _personValidatorMock.Verify(ps => ps.ValidateAsync(It.IsAny<Person>(), default), Times.Never());
            _personRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_AddressDoesNotExist_ReturnsFalse()
        {
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            var person = PersonBuilder.NewObject().DomainBuild();
            _personRepositoryMock.Setup(pr => pr.FindByIdAsync(personUpdateRequest.Id, UtilTools.MockIIncludableQuery<Person>(), true)).ReturnsAsync(person);
            _cepServiceMock.Setup(cs => cs.GetAddressFromCepAsync(personUpdateRequest.Address.ZipCode));

            var serviceResult = await _personService.UpdateAsync(personUpdateRequest);

            _personRepositoryMock.Verify(pr => pr.FindByIdAsync(personUpdateRequest.Id, UtilTools.MockIIncludableQuery<Person>(), true), Times.Once());
            _cepServiceMock.Verify(cs => cs.GetAddressFromCepAsync(personUpdateRequest.Address.ZipCode), Times.Once());
            _personValidatorMock.Verify(ps => ps.ValidateAsync(It.IsAny<Person>(), default), Times.Never());
            _personRepositoryMock.Verify(ps => ps.UpdateAsync(It.IsAny<Person>()), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_EntityInvalid_ReturnsFalse()
        {
            var personUpdateRequest = PersonBuilder.NewObject().WithName("aa").UpdateRequestBuild();
            var person = PersonBuilder.NewObject().DomainBuild();
            var address = AddressBuilder.NewObject().DomainBuild();
            var validationFailureList = new List<ValidationFailure>()
            {
                new ValidationFailure(){ErrorMessage = "random error"},
                new ValidationFailure(){ErrorMessage = "error random"}
            };
            var validationResult = new ValidationResult()
            {
                Errors = validationFailureList
            };
            var validationFailureListCount = validationFailureList.Count;
            _personRepositoryMock.Setup(r => r.FindByIdAsync(personUpdateRequest.Id, UtilTools.MockIIncludableQuery<Person>(), true)).ReturnsAsync(person);
            _cepServiceMock.Setup(cs => cs.GetAddressFromCepAsync(personUpdateRequest.Address.ZipCode)).ReturnsAsync(address);
            _personValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Person>(), default)).ReturnsAsync(validationResult);
            AddDomainNotificationsInRange(validationFailureListCount);

            var serviceResult = await _personService.UpdateAsync(personUpdateRequest);

            _personRepositoryMock.Verify(r => r.FindByIdAsync(personUpdateRequest.Id, UtilTools.MockIIncludableQuery<Person>(), true), Times.Once());
            _cepServiceMock.Verify(cs => cs.GetAddressFromCepAsync(personUpdateRequest.Address.ZipCode), Times.Once());
            _personValidatorMock.Verify(pv => pv.ValidateAsync(It.IsAny<Person>(), default), Times.Once());
            VerifyNotificationHandlerInRange(validationFailureListCount);
            _personRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            var id = 1;
            _personRepositoryMock.Setup(r => r.HaveObjectInDbAsync(r => r.Id == id)).ReturnsAsync(true);
            _personRepositoryMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var serviceResult = await _personService.DeleteAsync(id);

            _personRepositoryMock.Verify(r => r.HaveObjectInDbAsync(r => r.Id == id), Times.Once());
            _personRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once());
            Assert.True(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            var id = 1;
            _personRepositoryMock.Setup(r => r.HaveObjectInDbAsync(r => r.Id == id)).ReturnsAsync(false);

            var serviceResult = await _personService.DeleteAsync(id);

            _personRepositoryMock.Verify(r => r.HaveObjectInDbAsync(r => r.Id == id), Times.Once());
            _personRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Never());
            Assert.False(serviceResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            var id = 1;
            var person = PersonBuilder.NewObject().DomainBuild();
            _personRepositoryMock.Setup(r => r.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Person>(), false)).ReturnsAsync(person);

            var serviceResult = await _personService.FindByIdAsync(id);

            _personRepositoryMock.Verify(r => r.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Person>(), false), Times.Once());
            Assert.NotNull(serviceResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsNull()
        {
            var id = 1;
            _personRepositoryMock.Setup(r => r.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Person>(), false));

            var serviceResult = await _personService.FindByIdAsync(id);

            _personRepositoryMock.Verify(r => r.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Person>(), false), Times.Once());
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
            _personRepositoryMock.Setup(r => r.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Person>())).ReturnsAsync(personList);

            var serviceResult = await _personService.FindAllEntitiesAsync();

            _personRepositoryMock.Verify(r => r.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Person>()), Times.Once());
            Assert.Equal(serviceResult.Count, personList.Count);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEmptyList()
        {
            _personRepositoryMock.Setup(r => r.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Person>()));

            var serviceResult = await _personService.FindAllEntitiesAsync();

            _personRepositoryMock.Verify(r => r.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Person>()), Times.Once());
            Assert.Empty(serviceResult);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            var personList = new List<Person>()
            {
                PersonBuilder.NewObject().DomainBuild(),
                PersonBuilder.NewObject().DomainBuild(),
                PersonBuilder.NewObject().DomainBuild()
            };
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();
            var personPageList = new PageList<Person>(personList, personList.Count, pageParams);
            _personRepositoryMock.Setup(pr => pr.FindAllEntitiesWithPaginationAsync(pageParams, UtilTools.MockIIncludableQuery<Person>())).ReturnsAsync(personPageList);

            var serviceResult = await _personService.FindAllEntitiesWithPaginationAsync(pageParams);

            _personRepositoryMock.Verify(pr => pr.FindAllEntitiesWithPaginationAsync(pageParams, UtilTools.MockIIncludableQuery<Person>()), Times.Once());
            Assert.Equal(serviceResult.Result.Count, personPageList.Result.Count);
        }

        private void AddDomainNotificationsInRange(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _notificationMock.Setup(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            }
        }

        private void VerifyNotificationHandlerInRange(int numberOfTimesToVerify) =>
            _notificationMock.Verify(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(numberOfTimesToVerify));
    }
}
