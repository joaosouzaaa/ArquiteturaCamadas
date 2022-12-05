using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Post;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Services;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using Azure;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Hosting;
using Moq;
using TestBuilders;
using TestBuilders.Helpers;

namespace UnitTests.ServiceTests
{
    public sealed class PostServiceTests
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<ITagService> _tagServiceMock;
        private readonly Mock<IValidator<Post>> _postValidatorMock;
        private readonly Mock<INotificationHandler> _notificationHandlerMock;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _tagServiceMock = new Mock<ITagService>();
            _postValidatorMock = new Mock<IValidator<Post>>();
            _notificationHandlerMock = new Mock<INotificationHandler>();
            _postService = new PostService(_postRepositoryMock.Object, _tagServiceMock.Object,
                                           _postValidatorMock.Object, _notificationHandlerMock.Object);

            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            // A
            var tagsIds = new List<int>()
            {
                1, 2, 3
            };
            var postSaveRequest = PostBuilder.NewObject().WithTagsIds(tagsIds).SaveRequestBuild();
            var validationResult = new ValidationResult();

            _postValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Post>(), default)).ReturnsAsync(validationResult);

            foreach (var tagId in tagsIds)
            {
                var tag = TagBuilder.NewObject().DomainBuild();

                _tagServiceMock.Setup(ts => ts.FindByIdAsyncAsNoTrackingReturnsDomainObject(tagId)).ReturnsAsync(tag);
            }

            _postRepositoryMock.Setup(pr => pr.AddAsync(It.IsAny<Post>())).ReturnsAsync(true);

            // A
            var serviceResult = await _postService.AddAsync(postSaveRequest);

            // A
            VerifyAddAsyncMockTimes(addNotificationMockTimes: 0, validateMockTimes: 1, findTagMockTimes: tagsIds.Count, addAsyncMockTimes: 1);

            Assert.True(serviceResult);
        }

        [Fact]
        public async Task AddAsync_InvalidImageFormat_ReturnsFalse()
        {
            // A
            var invalidImage = UtilTools.BuildInvalidIFormFileImageFormat();
            var postSaveRequest = PostBuilder.NewObject().WithImage(invalidImage).SaveRequestBuild();

            // A
            var serviceResult = await _postService.AddAsync(postSaveRequest);

            // A
            VerifyAddAsyncMockTimes(addNotificationMockTimes: 1, validateMockTimes: 0, findTagMockTimes: 0, addAsyncMockTimes: 0);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task AddAsync_EntityInvalid_ReturnsFalse()
        {
            // A
            var postSaveRequest = PostBuilder.NewObject().SaveRequestBuild();

            var validationFailureList = new List<ValidationFailure>()
            {
                new ValidationFailure()
                {
                    ErrorMessage = "joao"
                },
                new ValidationFailure()
                {
                    ErrorMessage = "karlos"
                }
            };

            var validationResult = new ValidationResult()
            {
                Errors = validationFailureList
            };

            _postValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Post>(), default)).ReturnsAsync(validationResult);

            AddDomainNotificationsInRange(validationFailureList.Count);

            // A
            var serviceResult = await _postService.AddAsync(postSaveRequest);

            // A
            VerifyAddAsyncMockTimes(addNotificationMockTimes: validationFailureList.Count, validateMockTimes: 1, findTagMockTimes: 0, addAsyncMockTimes: 0);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task AddAsync_TagDoesNotExist_ReturnsFalse()
        {
            // A
            var tagsIds = new List<int>()
            {
                1, 2
            };
            var postSaveRequest = PostBuilder.NewObject().WithTagsIds(tagsIds).SaveRequestBuild();
            var validationResult = new ValidationResult();

            _postValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Post>(), default)).ReturnsAsync(validationResult);
            _tagServiceMock.Setup(ts => ts.FindByIdAsyncAsNoTrackingReturnsDomainObject(It.IsAny<int>()));

            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _postService.AddAsync(postSaveRequest);

            // A
            VerifyAddAsyncMockTimes(addNotificationMockTimes: 1, validateMockTimes: 1, findTagMockTimes: 1, addAsyncMockTimes: 0);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateUnreapeatTagsSearchAsync_ReturnsTrue()
        {
            // A
            var tagsIds = new List<int>()
            {
                3, 2, 9
            };

            var postUpdateRequest = PostBuilder.NewObject().WithTagsIds(tagsIds).UpdateRequestBuild();

            var tagsList = new List<Tag>()
            {
                TagBuilder.NewObject().WithId(3).DomainBuild(),
                TagBuilder.NewObject().WithId(9).DomainBuild(),
            };

            var post = PostBuilder.NewObject().WithTagList(tagsList).DomainBuild();

            var validationResult = new ValidationResult();

            var tagsIdsList = post.Tags.Select(t => t.Id).ToList();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(postUpdateRequest.Id, UtilTools.MockIIncludableQuery<Post>(), false)).ReturnsAsync(post);
            _postValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Post>(), default)).ReturnsAsync(validationResult);
            _postRepositoryMock.Setup(pr => pr.UpdateAsync(It.IsAny<Post>())).ReturnsAsync(true);
            var findByTagIdMockTimes = SetupFindTagAndReturnCount(requestTagsIds: postUpdateRequest.TagsIds, domainTagsIds: tagsIdsList);
            _postRepositoryMock.Setup(pr => pr.DetachEntityAndSaveChangesAsync(It.IsAny<Post>())).ReturnsAsync(true);

            // A
            var serviceResult = await _postService.UpdateUnreapeatTagsSearchAsync(postUpdateRequest);

            // A
            VerifyUpdateUnreapeatTagsSearchAsyncMockTimes(validateMockTimes: 1, updateAsyncMockTimes: 1, findByTagIdMockTimes,
                detachEntityMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(serviceResult);
        }

        [Fact]
        public async Task UpdateUnreapeatTagsSearchAsync_PostDoesNotExist_ReturnsFalse()
        {
            // A
            var postUpdateRequest = PostBuilder.NewObject().UpdateRequestBuild();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(postUpdateRequest.Id, UtilTools.MockIIncludableQuery<Post>(), false));

            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _postService.UpdateUnreapeatTagsSearchAsync(postUpdateRequest);

            // A
            VerifyUpdateUnreapeatTagsSearchAsyncMockTimes(validateMockTimes: 0, updateAsyncMockTimes: 0, findByTagIdMockTimes: 0,
                detachEntityMockTimes: 0, addNotificationMockTimes: 1);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateUnreapeatTagsSearchAsync_InvalidImageFormat_ReturnsFalse()
        {
            // A
            var invalidImage = UtilTools.BuildInvalidIFormFileImageFormat();
            var postUpdateRequest = PostBuilder.NewObject().WithImage(invalidImage).UpdateRequestBuild();

            var post = PostBuilder.NewObject().DomainBuild();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(postUpdateRequest.Id, UtilTools.MockIIncludableQuery<Post>(), false)).ReturnsAsync(post);

            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _postService.UpdateUnreapeatTagsSearchAsync(postUpdateRequest);

            // A
            VerifyUpdateUnreapeatTagsSearchAsyncMockTimes(validateMockTimes: 0, updateAsyncMockTimes: 0, findByTagIdMockTimes: 0,
                detachEntityMockTimes: 0, addNotificationMockTimes: 1);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateUnreapeatTagsSearchAsync_EntityInvalid_ReturnsFalse()
        {
            // A
            var postUpdateRequest = PostBuilder.NewObject().UpdateRequestBuild();

            var post = PostBuilder.NewObject().DomainBuild();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(postUpdateRequest.Id, UtilTools.MockIIncludableQuery<Post>(), false)).ReturnsAsync(post);

            var validationFailureList = new List<ValidationFailure>()
            {
                new ValidationFailure()
                {
                    ErrorMessage = "random"
                }
            };

            var validationResult = new ValidationResult()
            {
                Errors = validationFailureList
            };

            _postValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Post>(), default)).ReturnsAsync(validationResult);

            AddDomainNotificationsInRange(validationFailureList.Count);

            // A
            var serviceResult = await _postService.UpdateUnreapeatTagsSearchAsync(postUpdateRequest);

            //
            VerifyUpdateUnreapeatTagsSearchAsyncMockTimes(validateMockTimes: 1, updateAsyncMockTimes: 0, findByTagIdMockTimes: 0,
                detachEntityMockTimes: 0, addNotificationMockTimes: validationFailureList.Count);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateUnreapeatTagsSearchAsync_UpdateAsyncDoesNotSucceeded_ReturnsFalse()
        {
            // A
            var postUpdateRequest = PostBuilder.NewObject().UpdateRequestBuild();

            var post = PostBuilder.NewObject().DomainBuild();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(postUpdateRequest.Id, UtilTools.MockIIncludableQuery<Post>(), false)).ReturnsAsync(post);

            var validationResult = new ValidationResult();

            _postValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Post>(), default)).ReturnsAsync(validationResult);

            _postRepositoryMock.Setup(pr => pr.UpdateAsync(It.IsAny<Post>())).ReturnsAsync(false);

            // A
            var serviceResult = await _postService.UpdateUnreapeatTagsSearchAsync(postUpdateRequest);

            // A
            VerifyUpdateUnreapeatTagsSearchAsyncMockTimes(validateMockTimes: 1, updateAsyncMockTimes: 1, findByTagIdMockTimes: 0,
                detachEntityMockTimes: 0, addNotificationMockTimes: 0);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateUnreapeatTagsSearchAsync_TagDoesNotExist_ReturnsFalse()
        {
            // A
            var tagsIds = new List<int>()
            {
                5, 7
            };

            var postUpdateRequest = PostBuilder.NewObject().WithTagsIds(tagsIds).UpdateRequestBuild();

            var tagsList = new List<Tag>()
            {
                TagBuilder.NewObject().WithId(1).DomainBuild()
            };

            var post = PostBuilder.NewObject().WithTagList(tagsList).DomainBuild();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(postUpdateRequest.Id, UtilTools.MockIIncludableQuery<Post>(), false)).ReturnsAsync(post);

            var validationResult = new ValidationResult();

            _postValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Post>(), default)).ReturnsAsync(validationResult);

            _postRepositoryMock.Setup(pr => pr.UpdateAsync(It.IsAny<Post>())).ReturnsAsync(true);

            _tagServiceMock.Setup(ts => ts.FindByIdAsyncAsNoTrackingReturnsDomainObject(It.IsAny<int>()));

            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _postService.UpdateUnreapeatTagsSearchAsync(postUpdateRequest);

            // A
            VerifyUpdateUnreapeatTagsSearchAsyncMockTimes(validateMockTimes: 1, updateAsyncMockTimes: 1, findByTagIdMockTimes: 1,
                detachEntityMockTimes: 0, addNotificationMockTimes: 1);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateManyToManyAsync_ReturnsTrue()
        {
            // A
            var tagsIds = new List<int>()
            {
                1, 2, 3
            };
            var postUpdateRequest = PostBuilder.NewObject().WithTagsIds(tagsIds).UpdateRequestBuild();

            var post = PostBuilder.NewObject().DomainBuild();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(postUpdateRequest.Id, UtilTools.MockIIncludableQuery<Post>(), false)).ReturnsAsync(post);

            var validationResult = new ValidationResult();

            _postValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Post>(), default)).ReturnsAsync(validationResult);

            int findTagsAsyncMockTimes = 0;

            foreach (var tagId in postUpdateRequest.TagsIds)
            {
                var tag = TagBuilder.NewObject().DomainBuild();

                _tagServiceMock.Setup(ts => ts.FindByIdAsyncAsNoTrackingReturnsDomainObject(tagId)).ReturnsAsync(tag);

                findTagsAsyncMockTimes++;
            }

            _postRepositoryMock.Setup(pr => pr.UpdateAsync(It.IsAny<Post>())).ReturnsAsync(true);

            // A
            var serviceResult = await _postService.UpdateManyToManyAsync(postUpdateRequest);

            // A
            VerifyUpdateManyToManyAsyncMockTimes(validateAsyncMockTimes: 1, findTagsAsyncMockTimes, updateAsyncMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(serviceResult);
        }

        [Fact]
        public async Task UpdateManyToManyAsync_PostDoesNotExist_ReturnsFalse()
        {
            // A
            var postUpdateRequest = PostBuilder.NewObject().UpdateRequestBuild();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(postUpdateRequest.Id, UtilTools.MockIIncludableQuery<Post>(), false));

            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _postService.UpdateManyToManyAsync(postUpdateRequest);

            // A
            VerifyUpdateManyToManyAsyncMockTimes(validateAsyncMockTimes: 0, findTagsAsyncMockTimes: 0, updateAsyncMockTimes: 0, addNotificationMockTimes: 1);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateManyToManyAsync_InvalidImageFormat_ReturnsFalse()
        {
            // A
            var invalidImage = UtilTools.BuildInvalidIFormFileImageFormat();
            var postUpdateRequest = PostBuilder.NewObject().WithImage(invalidImage).UpdateRequestBuild();

            var post = PostBuilder.NewObject().DomainBuild();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(postUpdateRequest.Id, UtilTools.MockIIncludableQuery<Post>(), false)).ReturnsAsync(post);

            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _postService.UpdateManyToManyAsync(postUpdateRequest);

            // A
            VerifyUpdateManyToManyAsyncMockTimes(validateAsyncMockTimes: 0, findTagsAsyncMockTimes: 0, updateAsyncMockTimes: 0, addNotificationMockTimes: 1);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateManyToManyAsync_InvalidEntity_ReturnsFalse()
        {
            // A
            var postUpdateRequest = PostBuilder.NewObject().UpdateRequestBuild();

            var post = PostBuilder.NewObject().DomainBuild();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(postUpdateRequest.Id, UtilTools.MockIIncludableQuery<Post>(), false)).ReturnsAsync(post);

            var validationFailureList = new List<ValidationFailure>()
            {
                new ValidationFailure()
                {
                    ErrorMessage = "oi"
                },
                new ValidationFailure()
                {
                    ErrorMessage = "kkkkk"
                }
            };

            var validationResult = new ValidationResult()
            {
                Errors = validationFailureList
            };

            _postValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Post>(), default)).ReturnsAsync(validationResult);

            var validationFailureListCount = validationFailureList.Count;

            AddDomainNotificationsInRange(validationFailureListCount);

            // A
            var serviceResult = await _postService.UpdateManyToManyAsync(postUpdateRequest);

            // A
            VerifyUpdateManyToManyAsyncMockTimes(validateAsyncMockTimes: 1, findTagsAsyncMockTimes: 0, updateAsyncMockTimes: 0,
                addNotificationMockTimes: validationFailureListCount);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task UpdateManyToManyAsync_TagDoesNotExist_ReturnsFalse()
        {
            // A
            var tagsIds = new List<int>()
            {
                1, 2, 3
            };
            var postUpdateRequest = PostBuilder.NewObject().WithTagsIds(tagsIds).UpdateRequestBuild();

            var post = PostBuilder.NewObject().DomainBuild();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(postUpdateRequest.Id, UtilTools.MockIIncludableQuery<Post>(), false)).ReturnsAsync(post);

            var validationResult = new ValidationResult();

            _postValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Post>(), default)).ReturnsAsync(validationResult);

            var tag = TagBuilder.NewObject().DomainBuild();

            _tagServiceMock.SetupSequence(ts => ts.FindByIdAsyncAsNoTrackingReturnsDomainObject(It.IsAny<int>()))
                .ReturnsAsync(tag)
                .Returns(Task.FromResult<Tag>(null));

            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _postService.UpdateManyToManyAsync(postUpdateRequest);

            // A
            VerifyUpdateManyToManyAsyncMockTimes(validateAsyncMockTimes: 1, findTagsAsyncMockTimes: 2, updateAsyncMockTimes: 0, addNotificationMockTimes: 1);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // A
            var id = 1;

            _postRepositoryMock.Setup(pr => pr.HaveObjectInDbAsync(p => p.Id == id)).ReturnsAsync(true);
            _postRepositoryMock.Setup(pr => pr.DeleteAsync(id)).ReturnsAsync(true);

            // A
            var serviceResult = await _postService.DeleteAsync(id);

            // A
            VerifyDeleteAsyncMockTimes(id, deleteAsyncMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(serviceResult);
        }

        [Fact]
        public async Task DeleteAsync_EntityDoesNotExist_ReturnsFalse()
        {
            // A
            var id = 1;

            _postRepositoryMock.Setup(pr => pr.HaveObjectInDbAsync(p => p.Id == id)).ReturnsAsync(false);

            AddDomainNotificationsInRange(1);

            // A
            var serviceResult = await _postService.DeleteAsync(id);

            // A
            VerifyDeleteAsyncMockTimes(id, deleteAsyncMockTimes: 0, addNotificationMockTimes: 1);

            Assert.False(serviceResult);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            // A
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();

            var postList = BuildPostListInRandomRange();

            var postPageList = new PageList<Post>(postList, postList.Count, pageParams);

            _postRepositoryMock.Setup(pr => pr.FindAllEntitiesWithPaginationAsync(pageParams, UtilTools.MockIIncludableQuery<Post>())).ReturnsAsync(postPageList);

            // A
            var serviceResult = await _postService.FindAllEntitiesWithPaginationAsync(pageParams);

            // A
            _postRepositoryMock.Verify(pr => pr.FindAllEntitiesWithPaginationAsync(pageParams, UtilTools.MockIIncludableQuery<Post>()), Times.Once());

            Assert.Equal(serviceResult.Result.Count, postPageList.Result.Count);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            // A
            var postList = BuildPostListInRandomRange();
            
            _postRepositoryMock.Setup(pr => pr.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Post>())).ReturnsAsync(postList);

            // A
            var serviceResult = await _postService.FindAllEntitiesAsync();

            // A
            _postRepositoryMock.Verify(pr => pr.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Post>()), Times.Once());

            Assert.Equal(serviceResult.Count, postList.Count);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            // A
            var id = 1;

            var post = PostBuilder.NewObject().DomainBuild();

            _postRepositoryMock.Setup(pr => pr.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Post>(), false)).ReturnsAsync(post);

            // A
            var serviceResult = await _postService.FindByIdAsync(id);

            // A
            _postRepositoryMock.Verify(pr => pr.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Post>(), false), Times.Once());

            Assert.NotNull(serviceResult);
        }

        private void VerifyAddAsyncMockTimes(int addNotificationMockTimes, int validateMockTimes, int findTagMockTimes, int addAsyncMockTimes)
        {
            VerifyAddDomainNotificationMocks(addNotificationMockTimes);
            _postValidatorMock.Verify(pv => pv.ValidateAsync(It.IsAny<Post>(), default), Times.Exactly(validateMockTimes));
            _tagServiceMock.Verify(ts => ts.FindByIdAsyncAsNoTrackingReturnsDomainObject(It.IsAny<int>()), Times.Exactly(findTagMockTimes));
            _postRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Post>()), Times.Exactly(addAsyncMockTimes));
        }

        private int SetupFindTagAndReturnCount(List<int> requestTagsIds, List<int> domainTagsIds)
        {
            int findByTagIdMockTimes = 0;

            foreach (var tagId in requestTagsIds)
            {
                if (!domainTagsIds.Contains(tagId))
                {
                    var tag = TagBuilder.NewObject().DomainBuild();

                    _tagServiceMock.Setup(ts => ts.FindByIdAsyncAsNoTrackingReturnsDomainObject(It.IsAny<int>())).ReturnsAsync(tag);

                    findByTagIdMockTimes += 1;
                }
            }

            return findByTagIdMockTimes;
        }

        private void VerifyUpdateUnreapeatTagsSearchAsyncMockTimes(int validateMockTimes, int updateAsyncMockTimes, int findByTagIdMockTimes, int detachEntityMockTimes, int addNotificationMockTimes)
        {
            _postRepositoryMock.Verify(pr => pr.FindByIdAsync(It.IsAny<int>(), UtilTools.MockIIncludableQuery<Post>(), false), Times.Once());
            _postValidatorMock.Verify(pv => pv.ValidateAsync(It.IsAny<Post>(), default), Times.Exactly(validateMockTimes));
            _postRepositoryMock.Verify(pr => pr.UpdateAsync(It.IsAny<Post>()), Times.Exactly(updateAsyncMockTimes));
            _tagServiceMock.Verify(ts => ts.FindByIdAsyncAsNoTrackingReturnsDomainObject(It.IsAny<int>()), Times.Exactly(findByTagIdMockTimes));
            _postRepositoryMock.Verify(pr => pr.DetachEntityAndSaveChangesAsync(It.IsAny<Post>()), Times.Exactly(detachEntityMockTimes));
            VerifyAddDomainNotificationMocks(addNotificationMockTimes);
        }

        private void VerifyUpdateManyToManyAsyncMockTimes(int validateAsyncMockTimes, int findTagsAsyncMockTimes, int updateAsyncMockTimes, int addNotificationMockTimes)
        {
            _postRepositoryMock.Verify(pr => pr.FindByIdAsync(It.IsAny<int>(), UtilTools.MockIIncludableQuery<Post>(), false), Times.Once());
            _postValidatorMock.Verify(pv => pv.ValidateAsync(It.IsAny<Post>(), default), Times.Exactly(validateAsyncMockTimes));
            _tagServiceMock.Verify(ts => ts.FindByIdAsyncAsNoTrackingReturnsDomainObject(It.IsAny<int>()), Times.Exactly(findTagsAsyncMockTimes));
            _postRepositoryMock.Verify(pr => pr.UpdateAsync(It.IsAny<Post>()), Times.Exactly(updateAsyncMockTimes));
            VerifyAddDomainNotificationMocks(addNotificationMockTimes);
        }

        private void VerifyDeleteAsyncMockTimes(int id, int deleteAsyncMockTimes, int addNotificationMockTimes)
        {
            _postRepositoryMock.Verify(pr => pr.HaveObjectInDbAsync(p => p.Id == id), Times.Once());
            _postRepositoryMock.Verify(pr => pr.DeleteAsync(id), Times.Exactly(deleteAsyncMockTimes));
            VerifyAddDomainNotificationMocks(addNotificationMockTimes);
        }

        private List<Post> BuildPostListInRandomRange()
        {
            var listSize = new Random().Next(1, 20);
            var postList = new List<Post>();

            for (var i = 0; i < listSize; i++)
            {
                var post = PostBuilder.NewObject().DomainBuild();

                postList.Add(post);
            }

            return postList;
        }

        private void AddDomainNotificationsInRange(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _notificationHandlerMock.Setup(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            }
        }

        private void VerifyAddDomainNotificationMocks(int addNotificationMockTimes) =>
            _notificationHandlerMock.Verify(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(addNotificationMockTimes));
    }
}
