using ArquiteturaCamadas.Api.Controllers;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Student;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using Moq;
using TestBuilders;

namespace UnitTests.ControllerTests
{
    public sealed class StudentControllerTests
    {
        private readonly Mock<IStudentService> _studentServiceMock;
        private readonly StudentController _studentController;

        public StudentControllerTests()
        {
            _studentServiceMock = new Mock<IStudentService>();
            _studentController = new StudentController(_studentServiceMock.Object);
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            // A
            var studentSaveRequest = StudentBuilder.NewObject().SaveRequestBuild();

            _studentServiceMock.Setup(s => s.AddAsync(studentSaveRequest)).ReturnsAsync(true);

            // A
            var addResult = await _studentController.AddAsync(studentSaveRequest);

            // A
            _studentServiceMock.Verify(s => s.AddAsync(studentSaveRequest), Times.Once());

            Assert.True(addResult);
        }

        [Fact]
        public async Task AddAsync_ReturnsFalse()
        {
            // A
            var studentSaveRequest = StudentBuilder.NewObject().SaveRequestBuild();

            _studentServiceMock.Setup(s => s.AddAsync(studentSaveRequest)).ReturnsAsync(false);

            // A
            var addResult = await _studentController.AddAsync(studentSaveRequest);

            // A
            _studentServiceMock.Verify(s => s.AddAsync(studentSaveRequest), Times.Once());

            Assert.False(addResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            // A
            var studentUpdateRequest = StudentBuilder.NewObject().UpdateRequestBuild();

            _studentServiceMock.Setup(s => s.UpdateAsync(studentUpdateRequest)).ReturnsAsync(true);

            // A
            var updateResult = await _studentController.UpdateAsync(studentUpdateRequest);

            // A
            _studentServiceMock.Verify(s => s.UpdateAsync(studentUpdateRequest), Times.Once());

            Assert.True(updateResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse()
        {
            // A
            var studentUpdateRequest = StudentBuilder.NewObject().UpdateRequestBuild();

            _studentServiceMock.Setup(s => s.UpdateAsync(studentUpdateRequest)).ReturnsAsync(false);

            // A
            var updateResult = await _studentController.UpdateAsync(studentUpdateRequest);

            // A
            _studentServiceMock.Verify(s => s.UpdateAsync(studentUpdateRequest), Times.Once());

            Assert.False(updateResult);
        }

        [Fact]
        public async Task RemoveProjectAsync_ReturnsTrue()
        {
            // A
            var projectStudentRelationShipArgument = StudentBuilder.NewObject().RelationShipArgumentBuild();

            _studentServiceMock.Setup(s => s.RemoveProjectAsync(projectStudentRelationShipArgument)).ReturnsAsync(true);

            // A
            var removeProjectResult = await _studentController.RemoveProjectAsync(projectStudentRelationShipArgument);

            // A
            _studentServiceMock.Verify(s => s.RemoveProjectAsync(projectStudentRelationShipArgument), Times.Once());

            Assert.True(removeProjectResult);
        }

        [Fact]
        public async Task RemoveProjectAsync_ReturnsFalse()
        {
            // A
            var projectStudentRelationShipArgument = StudentBuilder.NewObject().RelationShipArgumentBuild();

            _studentServiceMock.Setup(s => s.RemoveProjectAsync(projectStudentRelationShipArgument)).ReturnsAsync(false);

            // A
            var removeProjectResult = await _studentController.RemoveProjectAsync(projectStudentRelationShipArgument);

            // A
            _studentServiceMock.Verify(s => s.RemoveProjectAsync(projectStudentRelationShipArgument), Times.Once());

            Assert.False(removeProjectResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // A
            var id = 1;

            _studentServiceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

            // A
            var deleteResult = await _studentController.DeleteAsync(id);

            // A
            _studentServiceMock.Verify(s => s.DeleteAsync(id), Times.Once());

            Assert.True(deleteResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse()
        {
            // A
            var id = 1;

            _studentServiceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

            // A
            var deleteResult = await _studentController.DeleteAsync(id);

            // A
            _studentServiceMock.Verify(s => s.DeleteAsync(id), Times.Once());

            Assert.False(deleteResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            // A
            var id = 1;
            var studentResponse = StudentBuilder.NewObject().ResponseBuild();

            _studentServiceMock.Setup(s => s.FindByIdAsync(id)).ReturnsAsync(studentResponse);

            // A
            var findStudentResult = await _studentController.FindByIdAsync(id);

            // A
            _studentServiceMock.Verify(s => s.FindByIdAsync(id), Times.Once());

            Assert.Equal(findStudentResult, studentResponse);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            // A
            var studentResponseList = GenerateStudentResponseListInRandomRange();

            _studentServiceMock.Setup(s => s.FindAllEntitiesAsync()).ReturnsAsync(studentResponseList);

            // A
            var findAllResult = await _studentController.FindAllEntitiesAsync();

            // A
            _studentServiceMock.Verify(s => s.FindAllEntitiesAsync(), Times.Once());

            Assert.Equal(findAllResult, studentResponseList);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            // A
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();
            var studentResponseList = GenerateStudentResponseListInRandomRange();
            var studentResponsePageList = new PageList<StudentResponse>(studentResponseList, studentResponseList.Count, pageParams);

            _studentServiceMock.Setup(s => s.FindAllEntitiesWithPaginationAsync(pageParams)).ReturnsAsync(studentResponsePageList);

            // A
            var findAllWithPaginationResult = await _studentController.FindAllEntitiesWithPaginationAsync(pageParams);

            // A
            _studentServiceMock.Verify(s => s.FindAllEntitiesWithPaginationAsync(pageParams), Times.Once());

            Assert.Equal(findAllWithPaginationResult, studentResponsePageList);
        }

        private List<StudentResponse> GenerateStudentResponseListInRandomRange()
        {
            var studentResponseList = new List<StudentResponse>();

            var randomRange = new Random().Next(1, 10);

            for(var i = 0; i < randomRange; i++)
            {
                var studentResponse = StudentBuilder.NewObject().ResponseBuild();
                
                studentResponseList.Add(studentResponse);
            }

            return studentResponseList;
        }
    }
}
