using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Tag;
using ArquiteturaCamadas.ApplicationService.Services;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TestBuilders;
using TestBuilders.Helpers;

namespace UnitTests.ServiceTests
{
    public sealed class TagServiceTests
    {
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<IValidator<Tag>> _tagValidatorMock;
        private readonly Mock<INotificationHandler> _notificationHandlerMock;
        private readonly TagService _tagService;

        public TagServiceTests()
        {
            _tagRepositoryMock = new Mock<ITagRepository>();
            _tagValidatorMock = new Mock<IValidator<Tag>>();
            _notificationHandlerMock = new Mock<INotificationHandler>();
            _tagService = new TagService(_tagRepositoryMock.Object, _tagValidatorMock.Object, _notificationHandlerMock.Object);

            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            // A
            var tagSaveRequest = TagBuilder.NewObject().SaveRequestBuild();
            var validationResult = new ValidationResult();

            _tagValidatorMock.Setup(tv => tv.ValidateAsync(It.IsAny<Tag>(), default)).ReturnsAsync(validationResult);
            _tagRepositoryMock.Setup(tr => tr.AddAsync(It.IsAny<Tag>())).ReturnsAsync(true);

            //A
            var serviceResult = await _tagService.AddAsync(tagSaveRequest);

            // A
            VerifyAddAsyncMocks(addAsyncMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(serviceResult);
        }

        [Fact]
        public async Task AddAsync_EntityInvalid_ReturnsFalse()
        {
            // A
            var tagSaveRequest = TagBuilder.NewObject().SaveRequestBuild();

            var validationFailureList = new List<ValidationFailure>()
            {
                new ValidationFailure(){ErrorMessage = "Error"},
                new ValidationFailure(){ErrorMessage = "random"}
            };

            var validationResult = new ValidationResult()
            {
                Errors = validationFailureList
            };

            _tagValidatorMock.Setup(tv => tv.ValidateAsync(It.IsAny<Tag>(), default)).ReturnsAsync(validationResult);
            AddDomainNotificationsInRange(validationFailureList.Count);

            // A
            var serviceResult = await _tagService.AddAsync(tagSaveRequest);

            // A
            VerifyAddAsyncMocks(addAsyncMockTimes: 0, addNotificationMockTimes: validationFailureList.Count);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            // A
            var tagUpdateRequest = TagBuilder.NewObject().UpdateRequestBuild();
            var tag = TagBuilder.NewObject().DomainBuild();
            var validationResult = new ValidationResult();

            _tagRepositoryMock.Setup(tr => tr.FindByIdAsync(It.IsAny<int>(), null, true)).ReturnsAsync(tag);
            _tagValidatorMock.Setup(tv => tv.ValidateAsync(It.IsAny<Tag>(), default)).ReturnsAsync(validationResult);
            _tagRepositoryMock.Setup(tr => tr.UpdateAsync(It.IsAny<Tag>())).ReturnsAsync(true);

            // A
            var serviceResult = await _tagService.UpdateAsync(tagUpdateRequest);

            // A
            VerifyUpdateAsyncMocks(validateMockTimes: 1, updateAsyncMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_TagDoesNotExist_ReturnsFalse()
        {
            // A
            var tagUpdateRequest = TagBuilder.NewObject().UpdateRequestBuild();

            _tagRepositoryMock.Setup(tr => tr.FindByIdAsync(It.IsAny<int>(), null, true));
            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _tagService.UpdateAsync(tagUpdateRequest);

            // A
            VerifyUpdateAsyncMocks(validateMockTimes: 0, updateAsyncMockTimes: 0, addNotificationMockTimes: 1);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateAsync_EntityInvalid_ReturnsFalse()
        {
            // A
            var tagUpdateRequest = TagBuilder.NewObject().UpdateRequestBuild();
            var tag = TagBuilder.NewObject().DomainBuild();

            var validationFailureList = new List<ValidationFailure>()
            {
                new ValidationFailure(){ErrorMessage = "Error"},
                new ValidationFailure(){ErrorMessage = "random"},
                new ValidationFailure(){ErrorMessage = "Oi"}
            };

            var validationResult = new ValidationResult()
            {
                Errors = validationFailureList
            };

            _tagRepositoryMock.Setup(tr => tr.FindByIdAsync(It.IsAny<int>(), null, true)).ReturnsAsync(tag);
            _tagValidatorMock.Setup(tv => tv.ValidateAsync(It.IsAny<Tag>(), default)).ReturnsAsync(validationResult);
            AddDomainNotificationsInRange(validationFailureList.Count);

            // A
            var serviceResult = await _tagService.UpdateAsync(tagUpdateRequest);

            // A
            VerifyUpdateAsyncMocks(validateMockTimes: 1, updateAsyncMockTimes: 0, addNotificationMockTimes: validationFailureList.Count);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // A
            var id = 1;

            _tagRepositoryMock.Setup(tr => tr.HaveObjectInDbAsync(t => t.Id == id)).ReturnsAsync(true);
            _tagRepositoryMock.Setup(tr => tr.DeleteAsync(id)).ReturnsAsync(true);

            // A
            var serviceResult = await _tagService.DeleteAsync(id);

            // A
            VerifyDeleteAsyncMocks(id, deleteAsyncMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_EntityDoesNotExist_ReturnsFalse()
        {
            // A
            var id = 1;

            _tagRepositoryMock.Setup(tr => tr.HaveObjectInDbAsync(t => t.Id == id)).ReturnsAsync(false);
            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _tagService.DeleteAsync(id);

            // A
            VerifyDeleteAsyncMocks(id, deleteAsyncMockTimes: 0, addNotificationMockTimes: 1);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task FindByAsync_ReturnsEntity()
        {
            // A 
            var id = 1;
            var tag = TagBuilder.NewObject().DomainBuild();

            _tagRepositoryMock.Setup(tr => tr.FindByIdAsync(id, null, false)).ReturnsAsync(tag);

            // A
            var serviceResult = await _tagService.FindByIdAsync(id);

            // A
            _tagRepositoryMock.Verify(tr => tr.FindByIdAsync(id, null, false), Times.Once());

            Assert.NotNull(serviceResult);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            // A
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();

            var tagList = new List<Tag>()
            {
                TagBuilder.NewObject().DomainBuild(),
                TagBuilder.NewObject().DomainBuild()
            };

            var tagPageList = new PageList<Tag>(tagList, tagList.Count, pageParams);

            _tagRepositoryMock.Setup(tr => tr.FindAllEntitiesWithPaginationAsync(pageParams, UtilTools.MockIIncludableQuery<Tag>())).ReturnsAsync(tagPageList);

            // A
            var serviceResult = await _tagService.FindAllEntitiesWithPaginationAsync(pageParams);

            // A
            _tagRepositoryMock.Verify(tr => tr.FindAllEntitiesWithPaginationAsync(pageParams, UtilTools.MockIIncludableQuery<Tag>()), Times.Once());

            Assert.Equal(serviceResult.Result.Count, tagPageList.Result.Count);
        }

        [Fact]
        public async Task FindByIdAsyncAsNoTrackingReturnsDomainObject_ReturnsEntity()
        {
            // A
            var id = 1;
            var tag = TagBuilder.NewObject().DomainBuild();

            _tagRepositoryMock.Setup(tr => tr.FindByIdAsync(id, null, false)).ReturnsAsync(tag);

            // A
            var serviceResult = await _tagService.FindByIdAsyncAsNoTrackingReturnsDomainObject(id);

            // A
            _tagRepositoryMock.Verify(tr => tr.FindByIdAsync(id, null, false), Times.Once());

            Assert.NotNull(serviceResult);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            // A
            var tagList = new List<Tag>()
            {
                TagBuilder.NewObject().DomainBuild(),
                TagBuilder.NewObject().DomainBuild(),
                TagBuilder.NewObject().DomainBuild()
            };

            _tagRepositoryMock.Setup(tr => tr.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Tag>())).ReturnsAsync(tagList);

            // A
            var serviceResult = await _tagService.FindAllEntitiesAsync();

            // A
            _tagRepositoryMock.Verify(tr => tr.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Tag>()), Times.Once());

            Assert.Equal(serviceResult.Count, tagList.Count);
        }

        private void AddDomainNotificationsInRange(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _notificationHandlerMock.Setup(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            }
        }

        private void VerifyAddAsyncMocks(int addAsyncMockTimes, int addNotificationMockTimes)
        {
            _tagValidatorMock.Verify(tv => tv.ValidateAsync(It.IsAny<Tag>(), default), Times.Once());
            _tagRepositoryMock.Verify(tr => tr.AddAsync(It.IsAny<Tag>()), Times.Exactly(addAsyncMockTimes));
            VerifyAddDomainNotificationMocks(addNotificationMockTimes);
        }

        private void VerifyUpdateAsyncMocks(int validateMockTimes, int updateAsyncMockTimes, int addNotificationMockTimes)
        {
            _tagRepositoryMock.Verify(tr => tr.FindByIdAsync(It.IsAny<int>(), null, true), Times.Once());
            _tagValidatorMock.Verify(tv => tv.ValidateAsync(It.IsAny<Tag>(), default), Times.Exactly(validateMockTimes));
            _tagRepositoryMock.Verify(tr => tr.UpdateAsync(It.IsAny<Tag>()), Times.Exactly(updateAsyncMockTimes));
            VerifyAddDomainNotificationMocks(addNotificationMockTimes);
        }

        private void VerifyDeleteAsyncMocks(int id, int deleteAsyncMockTimes, int addNotificationMockTimes)
        {
            _tagRepositoryMock.Verify(tr => tr.HaveObjectInDbAsync(t => t.Id == id), Times.Once());
            _tagRepositoryMock.Verify(tr => tr.DeleteAsync(id), Times.Exactly(deleteAsyncMockTimes));
            VerifyAddDomainNotificationMocks(addNotificationMockTimes);
        }

        private void VerifyAddDomainNotificationMocks(int addNotificationMockTimes) =>
            _notificationHandlerMock.Verify(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(addNotificationMockTimes));
    }
}
