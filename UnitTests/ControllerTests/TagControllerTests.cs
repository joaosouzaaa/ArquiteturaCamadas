using ArquiteturaCamadas.Api.Controllers;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using Moq;
using TestBuilders;

namespace UnitTests.ControllerTests
{
    public sealed class TagControllerTests
    {
        private readonly Mock<ITagService> _tagServiceMock;
        private readonly TagController _tagController;

        public TagControllerTests()
        {
            _tagServiceMock = new Mock<ITagService>();
            _tagController = new TagController(_tagServiceMock.Object);
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            var tagSaveRequest = TagBuilder.NewObject().SaveRequestBuild();
            _tagServiceMock.Setup(ts => ts.AddAsync(tagSaveRequest)).ReturnsAsync(true);

            var controllerResult = await _tagController.AddAsync(tagSaveRequest);

            _tagServiceMock.Verify(ts => ts.AddAsync(tagSaveRequest), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task AddAsync_ReturnsFalse()
        {
            var tagSaveRequest = TagBuilder.NewObject().SaveRequestBuild();
            _tagServiceMock.Setup(ts => ts.AddAsync(tagSaveRequest)).ReturnsAsync(false);

            var controllerResult = await _tagController.AddAsync(tagSaveRequest);

            _tagServiceMock.Verify(ts => ts.AddAsync(tagSaveRequest), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            var tagUpdateRequest = TagBuilder.NewObject().UpdateRequestBuild();
            _tagServiceMock.Setup(ts => ts.UpdateAsync(tagUpdateRequest)).ReturnsAsync(true);

            var controllerResult = await _tagController.UpdateAsync(tagUpdateRequest);

            _tagServiceMock.Verify(ts => ts.UpdateAsync(tagUpdateRequest), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse()
        {
            var tagUpdateRequest = TagBuilder.NewObject().UpdateRequestBuild();
            _tagServiceMock.Setup(ts => ts.UpdateAsync(tagUpdateRequest)).ReturnsAsync(false);

            var controllerResult = await _tagController.UpdateAsync(tagUpdateRequest);

            _tagServiceMock.Verify(ts => ts.UpdateAsync(tagUpdateRequest), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            var id = 1;
            _tagServiceMock.Setup(ts => ts.DeleteAsync(id)).ReturnsAsync(true);

            var controllerResult = await _tagController.DeleteAsync(id);

            _tagServiceMock.Verify(ts => ts.DeleteAsync(id), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            var id = 1;
            _tagServiceMock.Setup(ts => ts.DeleteAsync(id)).ReturnsAsync(false);

            var controllerResult = await _tagController.DeleteAsync(id);

            _tagServiceMock.Verify(ts => ts.DeleteAsync(id), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            var id = 1;
            var tagResponse = TagBuilder.NewObject().ResponseBuild();
            _tagServiceMock.Setup(ts => ts.FindByIdAsync(id)).ReturnsAsync(tagResponse);

            var controllerResult = await _tagController.FindByIdAsync(id);

            _tagServiceMock.Verify(ts => ts.FindByIdAsync(id), Times.Once());
            Assert.Equal(controllerResult, tagResponse);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            var randomNumberFrom1To20 = new Random().Next(1, 20);
            var tagPostsResponseList = CreateTagPostsResponseListInRange(randomNumberFrom1To20);
            _tagServiceMock.Setup(ts => ts.FindAllEntitiesAsync()).ReturnsAsync(tagPostsResponseList);

            var controllerResult = await _tagController.FindAllEntitiesAsync();

            _tagServiceMock.Verify(ts => ts.FindAllEntitiesAsync(), Times.Once());
            Assert.Equal(controllerResult, tagPostsResponseList);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            var randomNumberFrom1To10 = new Random().Next(1, 10);
            var tagPostsResponseList = CreateTagPostsResponseListInRange(randomNumberFrom1To10);
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();
            var tagPostsResponsePageList = new PageList<TagPostsResponse>(tagPostsResponseList, tagPostsResponseList.Count, pageParams);
            _tagServiceMock.Setup(ts => ts.FindAllEntitiesWithPaginationAsync(pageParams)).ReturnsAsync(tagPostsResponsePageList);

            var controllerResult = await _tagController.FindAllEntitiesWithPaginationAsync(pageParams);

            _tagServiceMock.Verify(ts => ts.FindAllEntitiesWithPaginationAsync(pageParams), Times.Once());
            Assert.Equal(controllerResult, tagPostsResponsePageList);
        }

        private List<TagPostsResponse> CreateTagPostsResponseListInRange(int range)
        {
            var tagPostsResponseList = new List<TagPostsResponse>();

            for (var i = 0; i < range; i++)
            {
                var tagPostsResponse = TagBuilder.NewObject().PostsResponseBuild();

                tagPostsResponseList.Add(tagPostsResponse);
            }

            return tagPostsResponseList;
        }
    }
}
