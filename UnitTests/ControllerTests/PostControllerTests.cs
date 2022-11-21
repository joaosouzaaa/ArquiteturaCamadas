﻿using ArquiteturaCamadas.Api.Controllers;
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
            var postSaveRequest = PostBuilder.NewObject().SaveRequestBuild();
            _postServiceMock.Setup(ps => ps.AddAsync(postSaveRequest)).ReturnsAsync(true);

            var controllerResult = await _postController.AddAsync(postSaveRequest);

            _postServiceMock.Verify(ps => ps.AddAsync(postSaveRequest), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task AddAsync_ReturnsFalse()
        {
            var postSaveRequest = PostBuilder.NewObject().SaveRequestBuild();
            _postServiceMock.Setup(ps => ps.AddAsync(postSaveRequest)).ReturnsAsync(false);

            var controllerResult = await _postController.AddAsync(postSaveRequest);

            _postServiceMock.Verify(ps => ps.AddAsync(postSaveRequest), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            var postUpdateRequest = PostBuilder.NewObject().UpdateRequestBuild();
            _postServiceMock.Setup(ps => ps.UpdateAsync(postUpdateRequest)).ReturnsAsync(true);

            var controllerResult = await _postController.UpdateAsync(postUpdateRequest);

            _postServiceMock.Verify(ps => ps.UpdateAsync(postUpdateRequest), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse()
        {
            var postUpdateRequest = PostBuilder.NewObject().UpdateRequestBuild();
            _postServiceMock.Setup(ps => ps.UpdateAsync(postUpdateRequest)).ReturnsAsync(false);

            var controllerResult = await _postController.UpdateAsync(postUpdateRequest);

            _postServiceMock.Verify(ps => ps.UpdateAsync(postUpdateRequest), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            var id = 1;
            _postServiceMock.Setup(ps => ps.DeleteAsync(id)).ReturnsAsync(true);

            var controllerResult = await _postController.DeleteAsync(id);

            _postServiceMock.Verify(ps => ps.DeleteAsync(id), Times.Once());
            Assert.True(controllerResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            var id = 1;
            _postServiceMock.Setup(ps => ps.DeleteAsync(id)).ReturnsAsync(false);

            var controllerResult = await _postController.DeleteAsync(id);

            _postServiceMock.Verify(ps => ps.DeleteAsync(id), Times.Once());
            Assert.False(controllerResult);
        }

        [Fact]
        public async Task FindAllEntitiesWithPagination_ReturnsEntities()
        {
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();
            var postTagsResponseList = new List<PostTagsResponse>()
            {
                PostBuilder.NewObject().TagsResponseBuild(),
                PostBuilder.NewObject().TagsResponseBuild(),
                PostBuilder.NewObject().TagsResponseBuild()
            };
            var postTagsResponsePageList = new PageList<PostTagsResponse>(postTagsResponseList, postTagsResponseList.Count, pageParams);
            _postServiceMock.Setup(ps => ps.FindAllEntitiesWithPaginationAsync(pageParams)).ReturnsAsync(postTagsResponsePageList);

            var controllerResult = await _postController.FindAllEntitiesWithPaginationAsync(pageParams);

            _postServiceMock.Verify(ps => ps.FindAllEntitiesWithPaginationAsync(pageParams), Times.Once());
            Assert.Equal(controllerResult, postTagsResponsePageList);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            var postTagsResponseList = new List<PostTagsResponse>()
            {
                PostBuilder.NewObject().TagsResponseBuild()
            };
            _postServiceMock.Setup(ps => ps.FindAllEntitiesAsync()).ReturnsAsync(postTagsResponseList);

            var controllerResult = await _postController.FindAllEntitiesAsync();

            _postServiceMock.Verify(ps => ps.FindAllEntitiesAsync(), Times.Once());
            Assert.Equal(controllerResult, postTagsResponseList);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            var id = 1;
            var postTagsResponse = PostBuilder.NewObject().TagsResponseBuild();
            _postServiceMock.Setup(ps => ps.FindByIdAsync(id)).ReturnsAsync(postTagsResponse);

            var controllerResult = await _postController.FindByIdAsync(id);

            _postServiceMock.Verify(ps => ps.FindByIdAsync(id), Times.Once());
            Assert.Equal(controllerResult, postTagsResponse);
        }
    }
}