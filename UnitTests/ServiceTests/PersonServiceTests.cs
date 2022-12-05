using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person;
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
            // A
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            var address = AddressBuilder.NewObject().DomainBuild();
            var validationResult = new ValidationResult();

            _cepServiceMock.Setup(cs => cs.GetAddressFromCepAsync(personSaveRequest.Address.ZipCode)).ReturnsAsync(address);
            _personValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Person>(), default)).ReturnsAsync(validationResult);
            _personRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Person>())).ReturnsAsync(true);

            // A
            var serviceResult = await _personService.AddAsync(personSaveRequest);

            // A
            VerifyAddAsyncMocks(validateAsyncMockTimes: 1, addAsyncMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(serviceResult);
        }

        [Fact]
        public async Task AddAsync_AddressDoesNotExist_ReturnsFalse()
        {
            // A
            var personSaveRequest = PersonBuilder.NewObject().SaveRequestBuild();
            
            _cepServiceMock.Setup(cs => cs.GetAddressFromCepAsync(personSaveRequest.Address.ZipCode));

            // A
            var serviceResult = await _personService.AddAsync(personSaveRequest);

            // A
            VerifyAddAsyncMocks(validateAsyncMockTimes: 0, addAsyncMockTimes: 0, addNotificationMockTimes: 0);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task AddAsync_EntityInvalid_ReturnsFalse()
        {
            // A
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

            // A
            var serviceResult = await _personService.AddAsync(personSaveRequest);

            // A
            VerifyAddAsyncMocks(validateAsyncMockTimes: 1, addAsyncMockTimes: 0, addNotificationMockTimes: validationFailureListCount);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            // A
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            var address = AddressBuilder.NewObject().DomainBuild();
            var validationResult = new ValidationResult();
            var person = PersonBuilder.NewObject().DomainBuild();

            _personRepositoryMock.Setup(r => r.FindByIdAsync(personUpdateRequest.Id, UtilTools.MockIIncludableQuery<Person>(), true)).ReturnsAsync(person);
            _cepServiceMock.Setup(cs => cs.GetAddressFromCepAsync(personUpdateRequest.Address.ZipCode)).ReturnsAsync(address);
            _personValidatorMock.Setup(ps => ps.ValidateAsync(It.IsAny<Person>(), default)).ReturnsAsync(validationResult);
            _personRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Person>())).ReturnsAsync(true);

            // A
            var serviceResult = await _personService.UpdateAsync(personUpdateRequest);

            // A
            VerifyUpdateAsyncMocks(getAddressMockTimes: 1, validateAsyncMockTimes: 1, 
                updateAsyncMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_EntityDoesNotExist_ReturnsFalse()
        {
            // A
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();

            _personRepositoryMock.Setup(r => r.FindByIdAsync(personUpdateRequest.Id, UtilTools.MockIIncludableQuery<Person>(), true));
            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _personService.UpdateAsync(personUpdateRequest);

            // A
            VerifyUpdateAsyncMocks(getAddressMockTimes: 0, validateAsyncMockTimes: 0,
                updateAsyncMockTimes: 0, addNotificationMockTimes: 1);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_AddressDoesNotExist_ReturnsFalse()
        {
            // A
            var personUpdateRequest = PersonBuilder.NewObject().UpdateRequestBuild();
            var person = PersonBuilder.NewObject().DomainBuild();

            _personRepositoryMock.Setup(pr => pr.FindByIdAsync(personUpdateRequest.Id, UtilTools.MockIIncludableQuery<Person>(), true)).ReturnsAsync(person);
            _cepServiceMock.Setup(cs => cs.GetAddressFromCepAsync(personUpdateRequest.Address.ZipCode));

            // A
            var serviceResult = await _personService.UpdateAsync(personUpdateRequest);

            // A
            VerifyUpdateAsyncMocks(getAddressMockTimes: 1, validateAsyncMockTimes: 0,
                updateAsyncMockTimes: 0, addNotificationMockTimes: 0);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_EntityInvalid_ReturnsFalse()
        {
            // A
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

            // A
            var serviceResult = await _personService.UpdateAsync(personUpdateRequest);

            // A
            VerifyUpdateAsyncMocks(getAddressMockTimes: 1, validateAsyncMockTimes: 1,
                updateAsyncMockTimes: 0, addNotificationMockTimes: validationFailureListCount);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // A
            var id = 1;

            _personRepositoryMock.Setup(r => r.HaveObjectInDbAsync(r => r.Id == id)).ReturnsAsync(true);
            _personRepositoryMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            // A
            var serviceResult = await _personService.DeleteAsync(id);

            // A
            VerifyDeleteAsyncMocks(id, deleteAsyncMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            // A
            var id = 1;

            _personRepositoryMock.Setup(r => r.HaveObjectInDbAsync(r => r.Id == id)).ReturnsAsync(false);
            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _personService.DeleteAsync(id);

            // A
            VerifyDeleteAsyncMocks(id, deleteAsyncMockTimes: 0, addNotificationMockTimes: 1);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            // A
            var id = 1;
            var person = PersonBuilder.NewObject().DomainBuild();

            _personRepositoryMock.Setup(r => r.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Person>(), false)).ReturnsAsync(person);

            // A
            var serviceResult = await _personService.FindByIdAsync(id);

            // A
            _personRepositoryMock.Verify(r => r.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Person>(), false), Times.Once());

            Assert.NotNull(serviceResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsNull()
        {
            // A
            var id = 1;

            _personRepositoryMock.Setup(r => r.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Person>(), false));

            // A
            var serviceResult = await _personService.FindByIdAsync(id);

            // A
            _personRepositoryMock.Verify(r => r.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Person>(), false), Times.Once());

            Assert.Null(serviceResult);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            // A
            var personList = new List<Person>()
            {
                PersonBuilder.NewObject().DomainBuild(),
                PersonBuilder.NewObject().DomainBuild()
            };

            _personRepositoryMock.Setup(r => r.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Person>())).ReturnsAsync(personList);

            // A
            var serviceResult = await _personService.FindAllEntitiesAsync();

            // A
            _personRepositoryMock.Verify(r => r.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Person>()), Times.Once());

            Assert.Equal(serviceResult.Count, personList.Count);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEmptyList()
        {
            // A
            _personRepositoryMock.Setup(r => r.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Person>()));

            // A
            var serviceResult = await _personService.FindAllEntitiesAsync();

            // A
            _personRepositoryMock.Verify(r => r.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Person>()), Times.Once());

            Assert.Empty(serviceResult);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            // A
            var personList = new List<Person>()
            {
                PersonBuilder.NewObject().DomainBuild(),
                PersonBuilder.NewObject().DomainBuild(),
                PersonBuilder.NewObject().DomainBuild()
            };

            var pageParams = PageParamsBuilder.NewObject().DomainBuild();

            var personPageList = new PageList<Person>(personList, personList.Count, pageParams);

            _personRepositoryMock.Setup(pr => pr.FindAllEntitiesWithPaginationAsync(pageParams, UtilTools.MockIIncludableQuery<Person>())).ReturnsAsync(personPageList);

            // A
            var serviceResult = await _personService.FindAllEntitiesWithPaginationAsync(pageParams);

            // A
            _personRepositoryMock.Verify(pr => pr.FindAllEntitiesWithPaginationAsync(pageParams, UtilTools.MockIIncludableQuery<Person>()), Times.Once());

            Assert.Equal(serviceResult.Result.Count, personPageList.Result.Count);
        }

        private void VerifyAddAsyncMocks(int validateAsyncMockTimes, int addAsyncMockTimes, int addNotificationMockTimes)
        {
            _cepServiceMock.Verify(cs => cs.GetAddressFromCepAsync(It.IsAny<string>()), Times.Once());
            _personValidatorMock.Verify(pv => pv.ValidateAsync(It.IsAny<Person>(), default), Times.Exactly(validateAsyncMockTimes));
            _personRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Exactly(addAsyncMockTimes));
            VerifyNotificationHandlerInRange(addNotificationMockTimes);
        }

        private void VerifyUpdateAsyncMocks(int getAddressMockTimes, int validateAsyncMockTimes, int updateAsyncMockTimes, int addNotificationMockTimes)
        {
            _personRepositoryMock.Verify(r => r.FindByIdAsync(It.IsAny<int>(), UtilTools.MockIIncludableQuery<Person>(), true), Times.Once());
            _cepServiceMock.Verify(cs => cs.GetAddressFromCepAsync(It.IsAny<string>()), Times.Exactly(getAddressMockTimes));
            _personValidatorMock.Verify(ps => ps.ValidateAsync(It.IsAny<Person>(), default), Times.Exactly(validateAsyncMockTimes));
            _notificationMock.Verify(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(addNotificationMockTimes));
            _personRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Exactly(updateAsyncMockTimes));
        }

        private void VerifyDeleteAsyncMocks(int id, int deleteAsyncMockTimes, int addNotificationMockTimes)
        {
            _personRepositoryMock.Verify(r => r.HaveObjectInDbAsync(r => r.Id == id), Times.Once());
            _personRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Exactly(deleteAsyncMockTimes));
            VerifyNotificationHandlerInRange(addNotificationMockTimes);
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
