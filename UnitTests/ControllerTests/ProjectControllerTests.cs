using ArquiteturaCamadas.Api.Controllers;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Project;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using Moq;
using TestBuilders;

namespace UnitTests.ControllerTests
{
    public sealed class ProjectControllerTests
    {
        private readonly Mock<IProjectService> _projectServiceMock;
        private readonly ProjectController _projectController;

        public ProjectControllerTests()
        {
            _projectServiceMock = new Mock<IProjectService>();
            _projectController = new ProjectController(_projectServiceMock.Object);
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            // A
            var projectSaveRequest = ProjectBuilder.NewObject().SaveRequestBuild();
            
            _projectServiceMock.Setup(ps => ps.AddAsync(projectSaveRequest)).ReturnsAsync(true);

            // A
            var addResult = await _projectController.AddAsync(projectSaveRequest);

            // A
            _projectServiceMock.Verify(ps => ps.AddAsync(projectSaveRequest), Times.Once());

            Assert.True(addResult);
        }

        [Fact]
        public async Task AddAsync_ReturnsFalse()
        {
            // A
            var projectSaveRequest = ProjectBuilder.NewObject().SaveRequestBuild();

            _projectServiceMock.Setup(ps => ps.AddAsync(projectSaveRequest)).ReturnsAsync(false);

            // A
            var addResult = await _projectController.AddAsync(projectSaveRequest);

            // A
            _projectServiceMock.Verify(ps => ps.AddAsync(projectSaveRequest), Times.Once());

            Assert.False(addResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            // A
            var projectUpdateRequest = ProjectBuilder.NewObject().UpdateRequestBuild();

            _projectServiceMock.Setup(ps => ps.UpdateAsync(projectUpdateRequest)).ReturnsAsync(true);

            // A
            var updateResult = await _projectController.UpdateAsync(projectUpdateRequest);

            // A
            _projectServiceMock.Verify(ps => ps.UpdateAsync(projectUpdateRequest), Times.Once());

            Assert.True(updateResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse()
        {
            // A
            var projectUpdateRequest = ProjectBuilder.NewObject().UpdateRequestBuild();

            _projectServiceMock.Setup(ps => ps.UpdateAsync(projectUpdateRequest)).ReturnsAsync(false);

            // A
            var updateResult = await _projectController.UpdateAsync(projectUpdateRequest);

            // A
            _projectServiceMock.Verify(ps => ps.UpdateAsync(projectUpdateRequest), Times.Once());

            Assert.False(updateResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // A
            var id = 1;

            _projectServiceMock.Setup(ps => ps.DeleteAsync(id)).ReturnsAsync(true);

            // A
            var deleteResult = await _projectController.DeleteAsync(id);

            // A
            _projectServiceMock.Verify(ps => ps.DeleteAsync(id), Times.Once());

            Assert.True(deleteResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            // A
            var id = 1;

            _projectServiceMock.Setup(ps => ps.DeleteAsync(id)).ReturnsAsync(false);

            // A
            var deleteResult = await _projectController.DeleteAsync(id);

            // A
            _projectServiceMock.Verify(ps => ps.DeleteAsync(id), Times.Once());

            Assert.False(deleteResult);
        }

        [Fact]
        public async Task FindProjectByIdAsync_ReturnsEntity()
        {
            // A
            var id = 1;
            var projectResponse = ProjectBuilder.NewObject().ResponseBuild();

            _projectServiceMock.Setup(ps => ps.FindProjectByIdAsync(id)).ReturnsAsync(projectResponse);

            // A
            var findResult = await _projectController.FindProjectByIdAsync(id);

            // A
            _projectServiceMock.Verify(ps => ps.FindProjectByIdAsync(id), Times.Once());

            Assert.Equal(findResult, projectResponse);
        }

        [Fact]
        public async Task FindAllProjectsAsync_ReturnsEntities()
        {
            // A
            var projectsResponseList = new List<ProjectResponse>()
            {
                ProjectBuilder.NewObject().ResponseBuild(),
                ProjectBuilder.NewObject().ResponseBuild(),
                ProjectBuilder.NewObject().ResponseBuild()
            };

            _projectServiceMock.Setup(ps => ps.FindAllProjectsAsync()).ReturnsAsync(projectsResponseList);

            // A
            var findAllResult = await _projectController.FindAllProjectsAsync();

            // A
            _projectServiceMock.Verify(ps => ps.FindAllProjectsAsync(), Times.Once());

            Assert.Equal(findAllResult, projectsResponseList);
        }
    }
}
