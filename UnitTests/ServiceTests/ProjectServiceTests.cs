using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Project;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Services;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Linq.Expressions;
using TestBuilders;
using TestBuilders.Helpers;

namespace UnitTests.ServiceTests
{
    public sealed class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly Mock<IStudentService> _studentServiceMock;
        private readonly Mock<IValidator<Project>> _projectValidatorMock;
        private readonly Mock<INotificationHandler> _notificationHandlerMock;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _studentServiceMock = new Mock<IStudentService>();
            _projectValidatorMock = new Mock<IValidator<Project>>();
            _notificationHandlerMock = new Mock<INotificationHandler>();
            _projectService = new ProjectService(_projectRepositoryMock.Object, _studentServiceMock.Object,
                                                 _projectValidatorMock.Object, _notificationHandlerMock.Object);

            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            // A
            var projectSaveRequest = ProjectBuilder.NewObject().SaveRequestBuild();

            _studentServiceMock.Setup(s => s.HaveStudentInDbAsync(projectSaveRequest.StudentId)).ReturnsAsync(true);

            var validationResult = new ValidationResult();

            _projectValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Project>(), default)).ReturnsAsync(validationResult);
            _projectRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(true);

            // A
            var addResult = await _projectService.AddAsync(projectSaveRequest);

            // A
            VerifyAddAsyncMocks(validateMockTimes: 1, addMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(addResult);
        }

        [Fact]
        public async Task AddAsync_StudentDoesNotExist_ReturnsFalse()
        {
            // A
            var projectSaveRequest = ProjectBuilder.NewObject().SaveRequestBuild();

            _studentServiceMock.Setup(s => s.HaveStudentInDbAsync(projectSaveRequest.StudentId)).ReturnsAsync(false);

            var addNotificationTimes = 1;
            SetupAddNotificationInRange(addNotificationTimes);

            // A
            var addResult = await _projectService.AddAsync(projectSaveRequest);

            // A
            VerifyAddAsyncMocks(validateMockTimes: 0, addMockTimes: 0, addNotificationTimes);

            Assert.False(addResult);
        }

        [Fact]
        public async Task AddAsync_EntityInvalid_ReturnsFalse()
        {
            // A
            var projectSaveRequest = ProjectBuilder.NewObject().SaveRequestBuild();

            _studentServiceMock.Setup(s => s.HaveStudentInDbAsync(projectSaveRequest.StudentId)).ReturnsAsync(true);

            var validationFailureList = new List<ValidationFailure>()
            {
                new ValidationFailure()
                {
                    ErrorMessage = "error"
                },
                new ValidationFailure()
                {
                    ErrorMessage = "message"
                },
                new ValidationFailure()
                {
                    ErrorMessage = "rn"
                },
            };

            var validationResult = new ValidationResult() 
            {
                Errors = validationFailureList
            };

            _projectValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<Project>(), default)).ReturnsAsync(validationResult);

            var addNotificationTimes = validationFailureList.Count;
            SetupAddNotificationInRange(addNotificationTimes);

            // A
            var addResult = await _projectService.AddAsync(projectSaveRequest);

            // A
            VerifyAddAsyncMocks(validateMockTimes: 1, addMockTimes: 0, addNotificationTimes);

            Assert.False(addResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            // A
            var projectUpdateRequest = ProjectBuilder.NewObject().UpdateRequestBuild();
            var project = ProjectBuilder.NewObject().DomainBuild();

            _projectRepositoryMock.Setup(r => r.FindByIdAsync(projectUpdateRequest.Id, UtilTools.MockIIncludableQuery<Project>(), true)).ReturnsAsync(project);
            _studentServiceMock.Setup(s => s.HaveStudentInDbAsync(projectUpdateRequest.StudentId)).ReturnsAsync(true);

            var validationResult = new ValidationResult();

            _projectValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Project>(), default)).ReturnsAsync(validationResult);
            _projectRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Project>())).ReturnsAsync(true);

            // A
            var updateResult = await _projectService.UpdateAsync(projectUpdateRequest);

            // A
            VerifyUpdateAsyncMocks(haveStudentMockTimes: 1, validateMockTimes: 1, updateMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(updateResult);
        }

        [Fact]
        public async Task UpdateAsync_ProjectDoesNotExist_ReturnsFalse()
        {
            // A
            var projectUpdateRequest = ProjectBuilder.NewObject().UpdateRequestBuild();

            _projectRepositoryMock.Setup(r => r.FindByIdAsync(projectUpdateRequest.Id, UtilTools.MockIIncludableQuery<Project>(), true));

            var addNotificationMockTimes = 1;
            SetupAddNotificationInRange(addNotificationMockTimes);

            // A
            var updateResult = await _projectService.UpdateAsync(projectUpdateRequest);

            // A
            VerifyUpdateAsyncMocks(haveStudentMockTimes: 0, validateMockTimes: 0, updateMockTimes: 0, addNotificationMockTimes);

            Assert.False(updateResult);
        }

        [Fact]
        public async Task UpdateAsync_StudentDoesNotExist_ReturnsFalse()
        {
            // A
            var projectUpdateRequest = ProjectBuilder.NewObject().UpdateRequestBuild();
            var project = ProjectBuilder.NewObject().DomainBuild();

            _projectRepositoryMock.Setup(r => r.FindByIdAsync(projectUpdateRequest.Id, UtilTools.MockIIncludableQuery<Project>(), true)).ReturnsAsync(project);
            _studentServiceMock.Setup(s => s.HaveStudentInDbAsync(projectUpdateRequest.StudentId)).ReturnsAsync(false);

            var addNotificationMockTimes = 1;
            SetupAddNotificationInRange(addNotificationMockTimes);

            // A
            var updateResult = await _projectService.UpdateAsync(projectUpdateRequest);

            // A
            VerifyUpdateAsyncMocks(haveStudentMockTimes: 1, validateMockTimes: 0, updateMockTimes: 0, addNotificationMockTimes);

            Assert.False(updateResult);
        }

        [Fact]
        public async Task UpdateAsync_EntityInvalid_ReturnsFalse()
        {
            // A
            var projectUpdateRequest = ProjectBuilder.NewObject().UpdateRequestBuild();
            var project = ProjectBuilder.NewObject().DomainBuild();

            _projectRepositoryMock.Setup(r => r.FindByIdAsync(projectUpdateRequest.Id, UtilTools.MockIIncludableQuery<Project>(), true)).ReturnsAsync(project);
            _studentServiceMock.Setup(s => s.HaveStudentInDbAsync(projectUpdateRequest.StudentId)).ReturnsAsync(true);

            var validationFailureList = new List<ValidationFailure>()
            {
                new ValidationFailure()
                {
                    ErrorMessage = "rnadom"
                }
            };

            var validationResult = new ValidationResult()
            {
                Errors = validationFailureList
            };

            _projectValidatorMock.Setup(pv => pv.ValidateAsync(It.IsAny<Project>(), default)).ReturnsAsync(validationResult);

            var addNotificationMockTimes = validationFailureList.Count;
            SetupAddNotificationInRange(addNotificationMockTimes);

            // A
            var updateResult = await _projectService.UpdateAsync(projectUpdateRequest);

            // A
            VerifyUpdateAsyncMocks(haveStudentMockTimes: 1, validateMockTimes: 1, updateMockTimes: 0, addNotificationMockTimes);

            Assert.False(updateResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // A
            var id = 1;

            _projectRepositoryMock.Setup(r => r.HaveObjectInDbAsync(p => p.Id == id)).ReturnsAsync(true);
            _projectRepositoryMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            // A
            var deleteResult = await _projectService.DeleteAsync(id);

            // A
            VerifyDeleteAsyncMockTimes(deleteMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(deleteResult);
        }

        [Fact]
        public async Task DeleteAsync_ProjectDoesNotExist_ReturnsFalse()
        {
            // A
            var id = 1;

            _projectRepositoryMock.Setup(r => r.HaveObjectInDbAsync(p => p.Id == id)).ReturnsAsync(false);

            var addNotificationMockTimes = 1;
            SetupAddNotificationInRange(addNotificationMockTimes);

            // A
            var deleteResult = await _projectService.DeleteAsync(id);

            // A
            VerifyDeleteAsyncMockTimes(deleteMockTimes: 0, addNotificationMockTimes);

            Assert.False(deleteResult);
        }

        [Fact]
        public async Task FindProjectByIdAsync_ReturnsEntity()
        {
            // A
            var id = 1;
            var project = ProjectBuilder.NewObject().DomainBuild();

            _projectRepositoryMock.Setup(r => r.FindByIdAsync(id, null, true)).ReturnsAsync(project);

            // A
            var findResult = await _projectService.FindProjectByIdAsync(id);

            // A
            _projectRepositoryMock.Verify(r => r.FindByIdAsync(id, null, true), Times.Once());

            Assert.NotNull(findResult);
        }

        [Fact]
        public async Task FindAllProjectsAsync_ReturnsEntities()
        {
            // A
            var projectsList = new List<Project>()
            {
                ProjectBuilder.NewObject().DomainBuild(),
                ProjectBuilder.NewObject().DomainBuild(),
                ProjectBuilder.NewObject().DomainBuild()
            };

            _projectRepositoryMock.Setup(r => r.FindAllEntitiesAsync(null)).ReturnsAsync(projectsList);

            // A
            var findAllResult = await _projectService.FindAllProjectsAsync();

            // A
            _projectRepositoryMock.Verify(r => r.FindAllEntitiesAsync(null), Times.Once());

            Assert.Equal(findAllResult.Count, projectsList.Count);
        }

        private void VerifyAddAsyncMocks(int validateMockTimes, int addMockTimes, int addNotificationMockTimes)
        {
            _studentServiceMock.Verify(s => s.HaveStudentInDbAsync(It.IsAny<int>()), Times.Once());
            _projectValidatorMock.Verify(pv => pv.ValidateAsync(It.IsAny<Project>(), default), Times.Exactly(validateMockTimes));
            _projectRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Exactly(addMockTimes));
            VerifyAddNotificationMock(addNotificationMockTimes);
        }

        private void VerifyUpdateAsyncMocks(int haveStudentMockTimes, int validateMockTimes, int updateMockTimes, int addNotificationMockTimes)
        {
            _projectRepositoryMock.Verify(r => r.FindByIdAsync(It.IsAny<int>(), UtilTools.MockIIncludableQuery<Project>(), true), Times.Once());
            _studentServiceMock.Verify(s => s.HaveStudentInDbAsync(It.IsAny<int>()), Times.Exactly(haveStudentMockTimes));
            _projectValidatorMock.Verify(pv => pv.ValidateAsync(It.IsAny<Project>(), default), Times.Exactly(validateMockTimes));
            _projectRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Project>()), Times.Exactly(updateMockTimes));
            VerifyAddNotificationMock(addNotificationMockTimes);
        }

        private void VerifyDeleteAsyncMockTimes(int deleteMockTimes, int addNotificationMockTimes)
        {
            _projectRepositoryMock.Verify(r => r.HaveObjectInDbAsync(It.IsAny<Expression<Func<Project, bool>>>()), Times.Once());
            _projectRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Exactly(deleteMockTimes));
            VerifyAddNotificationMock(addNotificationMockTimes);
        }

        private void VerifyAddNotificationMock(int addNotificationMockTimes) =>
            _notificationHandlerMock.Verify(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(addNotificationMockTimes));

        private void SetupAddNotificationInRange(int range)
        {
            for(var i = 0; i < range; i++)
            {
                _notificationHandlerMock.Setup(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            }
        }
    }
}
