using ArquiteturaCamadas.Api.Controllers;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using Moq;
using TestBuilders;

namespace UnitTests.ControllerTests
{
    public sealed class PostControllerTests
    {
        private readonly Mock<IPostService> _postServiceMock;
        private readonly PostController _postController;

        public PostControllerTests()
        {
            _postServiceMock = new Mock<IPostService>();
            _postController = new PostController(_postServiceMock.Object);
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            // A
            var postSaveRequest = PostBuilder.NewObject().SaveRequestBuild();

            _postServiceMock.Setup(ps => ps.AddAsync(postSaveRequest)).ReturnsAsync(true);

            // A
            var controllerResult = await _postController.AddAsync(postSaveRequest);

            // A
            _postServiceMock.Verify(ps => ps.AddAsync(postSaveRequest), Times.Once());

            Assert.True(controllerResult);
        }

        [Fact]
        public async Task AddAsync_ReturnsFalse()
        {
            // A
            var postSaveRequest = PostBuilder.NewObject().SaveRequestBuild();

            _postServiceMock.Setup(ps => ps.AddAsync(postSaveRequest)).ReturnsAsync(false);

            // A
            var controllerResult = await _postController.AddAsync(postSaveRequest);

            // A
            _postServiceMock.Verify(ps => ps.AddAsync(postSaveRequest), Times.Once());

            Assert.False(controllerResult);
        }

        [Fact]
        public async Task UpdateUnreapeatTagsSearchAsync_ReturnsTrue()
        {
            // A
            var postUpdateRequest = PostBuilder.NewObject().UpdateRequestBuild();

            _postServiceMock.Setup(ps => ps.UpdateUnreapeatTagsSearchAsync(postUpdateRequest)).ReturnsAsync(true);

            // A
            var controllerResult = await _postController.UpdateUnreapeatTagsSearchAsync(postUpdateRequest);

            // A
            _postServiceMock.Verify(ps => ps.UpdateUnreapeatTagsSearchAsync(postUpdateRequest), Times.Once());

            Assert.True(controllerResult);
        }

        [Fact]
        public async Task UpdateUnreapeatTagsSearchAsync_ReturnsFalse()
        {
            // A
            var postUpdateRequest = PostBuilder.NewObject().UpdateRequestBuild();

            _postServiceMock.Setup(ps => ps.UpdateUnreapeatTagsSearchAsync(postUpdateRequest)).ReturnsAsync(false);

            // A
            var controllerResult = await _postController.UpdateUnreapeatTagsSearchAsync(postUpdateRequest);

            // A
            _postServiceMock.Verify(ps => ps.UpdateUnreapeatTagsSearchAsync(postUpdateRequest), Times.Once());

            Assert.False(controllerResult);
        }

        [Fact]
        public async Task UpdateManyToManyAsync_ReturnsTrue()
        {
            // A
            var postUpdateRequest = PostBuilder.NewObject().UpdateRequestBuild();

            _postServiceMock.Setup(ps => ps.UpdateManyToManyAsync(postUpdateRequest)).ReturnsAsync(true);

            // A
            var controllerResult = await _postController.UpdateManyToManyAsync(postUpdateRequest);

            // A
            _postServiceMock.Verify(ps => ps.UpdateManyToManyAsync(postUpdateRequest), Times.Once());

            Assert.True(controllerResult);
        }

        [Fact]
        public async Task UpdateManyToManyAsync_ReturnsFalse()
        {
            // A
            var postUpdateRequest = PostBuilder.NewObject().UpdateRequestBuild();

            _postServiceMock.Setup(ps => ps.UpdateManyToManyAsync(postUpdateRequest)).ReturnsAsync(false);

            // A
            var controllerResult = await _postController.UpdateManyToManyAsync(postUpdateRequest);

            // A
            _postServiceMock.Verify(ps => ps.UpdateManyToManyAsync(postUpdateRequest), Times.Once());

            Assert.False(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // A
            var id = 1;

            _postServiceMock.Setup(ps => ps.DeleteAsync(id)).ReturnsAsync(true);

            // A
            var controllerResult = await _postController.DeleteAsync(id);

            // A
            _postServiceMock.Verify(ps => ps.DeleteAsync(id), Times.Once());

            Assert.True(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            // A
            var id = 1;

            _postServiceMock.Setup(ps => ps.DeleteAsync(id)).ReturnsAsync(false);

            // A
            var controllerResult = await _postController.DeleteAsync(id);

            // A
            _postServiceMock.Verify(ps => ps.DeleteAsync(id), Times.Once());

            Assert.False(controllerResult);
        }

        [Fact]
        public async Task FindAllEntitiesWithPagination_ReturnsEntities()
        {
            // A
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();
            var postTagsResponseList = new List<PostTagsResponse>()
            {
                PostBuilder.NewObject().TagsResponseBuild(),
                PostBuilder.NewObject().TagsResponseBuild(),
                PostBuilder.NewObject().TagsResponseBuild()
            };
            var postTagsResponsePageList = new PageList<PostTagsResponse>(postTagsResponseList, postTagsResponseList.Count, pageParams);

            _postServiceMock.Setup(ps => ps.FindAllEntitiesWithPaginationAsync(pageParams)).ReturnsAsync(postTagsResponsePageList);

            // A
            var controllerResult = await _postController.FindAllEntitiesWithPaginationAsync(pageParams);

            // A
            _postServiceMock.Verify(ps => ps.FindAllEntitiesWithPaginationAsync(pageParams), Times.Once());

            Assert.Equal(controllerResult, postTagsResponsePageList);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            // A
            var postTagsResponseList = new List<PostTagsResponse>()
            {
                PostBuilder.NewObject().TagsResponseBuild()
            };

            _postServiceMock.Setup(ps => ps.FindAllEntitiesAsync()).ReturnsAsync(postTagsResponseList);

            // A
            var controllerResult = await _postController.FindAllEntitiesAsync();

            // A
            _postServiceMock.Verify(ps => ps.FindAllEntitiesAsync(), Times.Once());

            Assert.Equal(controllerResult, postTagsResponseList);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            // A
            var id = 1;
            var postTagsResponse = PostBuilder.NewObject().TagsResponseBuild();

            _postServiceMock.Setup(ps => ps.FindByIdAsync(id)).ReturnsAsync(postTagsResponse);

            // A
            var controllerResult = await _postController.FindByIdAsync(id);

            // A
            _postServiceMock.Verify(ps => ps.FindByIdAsync(id), Times.Once());

            Assert.Equal(controllerResult, postTagsResponse);
        }
    }
}
