using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Student;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Services;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Linq.Expressions;
using TestBuilders;
using TestBuilders.Helpers;

namespace UnitTests.ServiceTests
{
    public sealed class StudentServiceTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<ICepService> _cepServiceMock;
        private readonly Mock<IValidator<Student>> _studentValidatorMock;
        private readonly Mock<INotificationHandler> _notificationHandlerMock;
        private readonly StudentService _studentService;

        public StudentServiceTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _cepServiceMock = new Mock<ICepService>();
            _studentValidatorMock = new Mock<IValidator<Student>>();
            _notificationHandlerMock = new Mock<INotificationHandler>();
            _studentService = new StudentService(_studentRepositoryMock.Object, _cepServiceMock.Object,
                _studentValidatorMock.Object, _notificationHandlerMock.Object);

            AutoMapperSettings.Inicialize();
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue()
        {
            // A
            var studentSaveRequest = StudentBuilder.NewObject().SaveRequestBuild();
            var address = AddressBuilder.NewObject().DomainBuild();

            _cepServiceMock.Setup(c => c.GetAddressFromCepAsync(studentSaveRequest.Address.ZipCode)).ReturnsAsync(address);

            var validationResult = new ValidationResult();
            
            _studentValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<Student>(), default)).ReturnsAsync(validationResult);
            _studentRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Student>())).ReturnsAsync(true);

            // A
            var addResult = await _studentService.AddAsync(studentSaveRequest);

            // A
            VerifyAddAsyncMockTimes(validateMockTimes: 1, addMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(addResult);
        }

        [Fact]
        public async Task AddAsync_AddressDoesNotExist_ReturnsFalse()
        {
            // A
            var studentSaveRequest = StudentBuilder.NewObject().SaveRequestBuild();

            _cepServiceMock.Setup(c => c.GetAddressFromCepAsync(studentSaveRequest.Address.ZipCode));

            // A
            var addResult = await _studentService.AddAsync(studentSaveRequest);

            // A
            VerifyAddAsyncMockTimes(validateMockTimes: 0, addMockTimes: 0, addNotificationMockTimes: 0);

            Assert.False(addResult);
        }

        [Fact]
        public async Task AddAsync_EntityInvalid_ReturnsFalse()
        {
            // A
            var studentSaveRequest = StudentBuilder.NewObject().SaveRequestBuild();
            var address = AddressBuilder.NewObject().DomainBuild();

            _cepServiceMock.Setup(c => c.GetAddressFromCepAsync(studentSaveRequest.Address.ZipCode)).ReturnsAsync(address);

            var validationFailureList = new List<ValidationFailure>()
            {
                new ValidationFailure()
                {
                    ErrorMessage = "chanel"
                },
                new ValidationFailure()
                {
                    ErrorMessage = "random"
                }
            };

            var validationResult = new ValidationResult()
            { 
                Errors = validationFailureList
            };

            _studentValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<Student>(), default)).ReturnsAsync(validationResult);

            var addNotificationMockTimes = validationFailureList.Count;
            SetupAddNotificationInRange(addNotificationMockTimes);

            // A
            var addResult = await _studentService.AddAsync(studentSaveRequest);

            // A
            VerifyAddAsyncMockTimes(validateMockTimes: 1, addMockTimes: 0, addNotificationMockTimes);

            Assert.False(addResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue()
        {
            // A
            var studentUpdateRequest = StudentBuilder.NewObject().UpdateRequestBuild();

            var student = StudentBuilder.NewObject().DomainBuild();

            _studentRepositoryMock.Setup(r => r.FindByIdAsync(studentUpdateRequest.Id, UtilTools.MockIIncludableQuery<Student>(), true)).ReturnsAsync(student);

            var address = AddressBuilder.NewObject().DomainBuild();

            _cepServiceMock.Setup(c => c.GetAddressFromCepAsync(studentUpdateRequest.Address.ZipCode)).ReturnsAsync(address);

            var validationResult = new ValidationResult();

            _studentValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<Student>(), default)).ReturnsAsync(validationResult);
            _studentRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Student>())).ReturnsAsync(true);

            // A
            var updateResult = await _studentService.UpdateAsync(studentUpdateRequest);

            // A
            VerifyUpdateAsyncMockTimes(getAddressMockTimes: 1, validateMockTimes: 1, updateMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(updateResult);
        }

        [Fact]
        public async Task UpdateAsync_StudentDoesNotExist_ReturnsFalse()
        {
            // A
            var studentUpdateRequest = StudentBuilder.NewObject().UpdateRequestBuild();

            _studentRepositoryMock.Setup(r => r.FindByIdAsync(studentUpdateRequest.Id, UtilTools.MockIIncludableQuery<Student>(), true));

            var addNotificationMockTimes = 1;
            SetupAddNotificationInRange(addNotificationMockTimes);

            // A
            var updateResult = await _studentService.UpdateAsync(studentUpdateRequest);

            // A
            VerifyUpdateAsyncMockTimes(getAddressMockTimes: 0, validateMockTimes: 0, updateMockTimes: 0, addNotificationMockTimes);

            Assert.False(updateResult);
        }

        [Fact]
        public async Task UpdateAsync_AddressDoesNotExist_ReturnsFalse()
        {
            // A
            var studentUpdateRequest = StudentBuilder.NewObject().UpdateRequestBuild();

            var student = StudentBuilder.NewObject().DomainBuild();

            _studentRepositoryMock.Setup(r => r.FindByIdAsync(studentUpdateRequest.Id, UtilTools.MockIIncludableQuery<Student>(), true)).ReturnsAsync(student);

            _cepServiceMock.Setup(c => c.GetAddressFromCepAsync(studentUpdateRequest.Address.ZipCode));

            // A
            var updateResult = await _studentService.UpdateAsync(studentUpdateRequest);

            // A
            VerifyUpdateAsyncMockTimes(getAddressMockTimes: 1, validateMockTimes: 0, updateMockTimes: 0, addNotificationMockTimes: 0);

            Assert.False(updateResult);
        }

        [Fact]
        public async Task UpdateAsync_EntityInvalid_ReturnsFalse()
        {
            // A
            var studentUpdateRequest = StudentBuilder.NewObject().UpdateRequestBuild();

            var student = StudentBuilder.NewObject().DomainBuild();

            _studentRepositoryMock.Setup(r => r.FindByIdAsync(studentUpdateRequest.Id, UtilTools.MockIIncludableQuery<Student>(), true)).ReturnsAsync(student);

            var address = AddressBuilder.NewObject().DomainBuild();

            _cepServiceMock.Setup(c => c.GetAddressFromCepAsync(studentUpdateRequest.Address.ZipCode)).ReturnsAsync(address);

            var validationFailureList = new List<ValidationFailure>()
            {
                new ValidationFailure()
                {
                    ErrorMessage = "new error"
                },
                new ValidationFailure()
                {
                    ErrorMessage = "error"
                },
                new ValidationFailure()
                {
                    ErrorMessage = "error"
                },
                new ValidationFailure()
                {
                    ErrorMessage = "error"
                }
            };

            var validationResult = new ValidationResult()
            { 
                Errors = validationFailureList
            };

            _studentValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<Student>(), default)).ReturnsAsync(validationResult);

            var addNotificationMockTimes = validationFailureList.Count;
            SetupAddNotificationInRange(addNotificationMockTimes);

            // A
            var updateResult = await _studentService.UpdateAsync(studentUpdateRequest);

            // A
            VerifyUpdateAsyncMockTimes(getAddressMockTimes: 1, validateMockTimes: 1, updateMockTimes: 0, addNotificationMockTimes);

            Assert.False(updateResult);
        }

        [Fact]
        public async Task RemoveProjectAsync_ReturnsTrue()
        {
            // A
            var projectId = 1;
            var projectStudentRelationShipArgument = StudentBuilder.NewObject().WithProjectId(projectId).RelationShipArgumentBuild();

            var projectList = new List<Project>()
            {
                ProjectBuilder.NewObject().WithId(projectId).DomainBuild(),
                ProjectBuilder.NewObject().DomainBuild()
            };

            var student = StudentBuilder.NewObject().WithProjects(projectList).DomainBuild();

            _studentRepositoryMock.Setup(r => r.FindByIdAsync(projectStudentRelationShipArgument.StudentId, UtilTools.MockIIncludableQuery<Student>(), false))
                .ReturnsAsync(student);

            _studentRepositoryMock.Setup(r => r.UpdateRelationshipAsync(It.IsAny<Student>())).ReturnsAsync(true);

            // A
            var removeProjectResult = await _studentService.RemoveProjectAsync(projectStudentRelationShipArgument);

            // A
            RemoveProjectAsyncMockTimes(updateRelationShipMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(removeProjectResult);
        }

        [Fact]
        public async Task RemoveProjectAsync_StudentDoesNotExist_ReturnsFalse()
        {
            // A
            var projectId = 1;
            var projectStudentRelationShipArgument = StudentBuilder.NewObject().WithProjectId(projectId).RelationShipArgumentBuild();

            var student = StudentBuilder.NewObject().DomainBuild();

            _studentRepositoryMock.Setup(r => r.FindByIdAsync(projectStudentRelationShipArgument.StudentId, UtilTools.MockIIncludableQuery<Student>(), false));

            var addNotificationMockTimes = 1;
            SetupAddNotificationInRange(addNotificationMockTimes);

            // A
            var removeProjectResult = await _studentService.RemoveProjectAsync(projectStudentRelationShipArgument);

            // A
            RemoveProjectAsyncMockTimes(updateRelationShipMockTimes: 0, addNotificationMockTimes);

            Assert.False(removeProjectResult);
        }

        [Fact]
        public async Task RemoveProjectAsync_ProjectDoesExist_ReturnsFalse()
        {
            // A
            var projectId = 1;
            var projectStudentRelationShipArgument = StudentBuilder.NewObject().WithProjectId(projectId).RelationShipArgumentBuild();

            var student = StudentBuilder.NewObject().DomainBuild();

            _studentRepositoryMock.Setup(r => r.FindByIdAsync(projectStudentRelationShipArgument.StudentId, UtilTools.MockIIncludableQuery<Student>(), false))
                .ReturnsAsync(student);

            var addNotificationMockTimes = 1;
            SetupAddNotificationInRange(addNotificationMockTimes);

            // A
            var removeProjectResult = await _studentService.RemoveProjectAsync(projectStudentRelationShipArgument);

            // A
            RemoveProjectAsyncMockTimes(updateRelationShipMockTimes: 0, addNotificationMockTimes);

            Assert.False(removeProjectResult);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue()
        {
            // A
            var id = 1;
            
            _studentRepositoryMock.Setup(r => r.HaveObjectInDbAsync(s => s.Id == id)).ReturnsAsync(true);
            _studentRepositoryMock.Setup(r => r.DeleteStudentAsync(id)).ReturnsAsync(true);

            // A
            var deleteResult = await _studentService.DeleteAsync(id);

            // A
            DeleteAsyncMockTimes(deleteMockTimes: 1, addNotificationMockTimes: 0);

            Assert.True(deleteResult);
        }

        [Fact]
        public async Task DeleteAsync_StudentDoesNotExist_ReturnsFalse()
        {
            // A
            var id = 1;

            _studentRepositoryMock.Setup(r => r.HaveObjectInDbAsync(s => s.Id == id)).ReturnsAsync(false);

            var addNotificationMockTimes = 1;
            SetupAddNotificationInRange(addNotificationMockTimes);

            // A
            var deleteResult = await _studentService.DeleteAsync(id);

            // A
            DeleteAsyncMockTimes(deleteMockTimes: 0, addNotificationMockTimes);

            Assert.False(deleteResult);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsEntity()
        {
            // A
            var id = 1;
            var student = StudentBuilder.NewObject().DomainBuild();

            _studentRepositoryMock.Setup(r => r.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Student>(), true)).ReturnsAsync(student);

            // A
            var findResult = await _studentService.FindByIdAsync(id);

            // A
            _studentRepositoryMock.Verify(r => r.FindByIdAsync(id, UtilTools.MockIIncludableQuery<Student>(), true), Times.Once());

            Assert.NotNull(findResult);
        }

        [Fact]
        public async Task FindAllEntitiesAsync_ReturnsEntities()
        {
            // A
            var studentList = BuildStudentListInRandomRange();

            _studentRepositoryMock.Setup(r => r.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Student>())).ReturnsAsync(studentList);

            // A
            var findAllResult = await _studentService.FindAllEntitiesAsync();

            // A
            _studentRepositoryMock.Verify(r => r.FindAllEntitiesAsync(UtilTools.MockIIncludableQuery<Student>()), Times.Once());

            Assert.Equal(findAllResult.Count, studentList.Count);
        }

        [Fact]
        public async Task FindAllEntitiesWithPaginationAsync_ReturnsEntities()
        {
            // A
            var pageParams = PageParamsBuilder.NewObject().DomainBuild();

            var studentList = BuildStudentListInRandomRange();
            var studentPageList = new PageList<Student>(studentList, studentList.Count, pageParams);

            _studentRepositoryMock.Setup(r => r.FindAllEntitiesWithPaginationAsync(pageParams, UtilTools.MockIIncludableQuery<Student>())).ReturnsAsync(studentPageList);

            // A
            var findAllWithPaginationResult = await _studentService.FindAllEntitiesWithPaginationAsync(pageParams);

            // A
            _studentRepositoryMock.Verify(r => r.FindAllEntitiesWithPaginationAsync(pageParams, UtilTools.MockIIncludableQuery<Student>()), Times.Once());

            Assert.Equal(findAllWithPaginationResult.Result.Count, studentPageList.Result.Count);
        }

        [Fact]
        public async Task HaveStudentInDbAsync_ReturnsTrue()
        {
            // A
            var id = 1;

            _studentRepositoryMock.Setup(r => r.HaveObjectInDbAsync(s => s.Id == id)).ReturnsAsync(true);

            // A
            var haveStudentResult = await _studentService.HaveStudentInDbAsync(id);

            // A
            VerifyHaveStudentInDbAsyncMockTimes();

            Assert.True(haveStudentResult);
        }

        [Fact]
        public async Task HaveStudentInDbAsync_ReturnsFalse()
        {
            // A
            var id = 1;

            _studentRepositoryMock.Setup(r => r.HaveObjectInDbAsync(s => s.Id == id)).ReturnsAsync(false);

            // A
            var haveStudentResult = await _studentService.HaveStudentInDbAsync(id);

            // A
            VerifyHaveStudentInDbAsyncMockTimes();

            Assert.False(haveStudentResult);
        }

        private void VerifyAddAsyncMockTimes(int validateMockTimes, int addMockTimes, int addNotificationMockTimes)
        {
            _cepServiceMock.Verify(c => c.GetAddressFromCepAsync(It.IsAny<string>()), Times.Once());
            _studentValidatorMock.Verify(v => v.ValidateAsync(It.IsAny<Student>(), default), Times.Exactly(validateMockTimes));
            _studentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Exactly(addMockTimes));
            VerifyAddNotificationMock(addNotificationMockTimes);
        }

        private void VerifyUpdateAsyncMockTimes(int getAddressMockTimes, int validateMockTimes, int updateMockTimes, int addNotificationMockTimes)
        {
            _studentRepositoryMock.Verify(r => r.FindByIdAsync(It.IsAny<int>(), UtilTools.MockIIncludableQuery<Student>(), true), Times.Once());
            _cepServiceMock.Verify(c => c.GetAddressFromCepAsync(It.IsAny<string>()), Times.Exactly(getAddressMockTimes));
            _studentValidatorMock.Verify(v => v.ValidateAsync(It.IsAny<Student>(), default), Times.Exactly(validateMockTimes));
            _studentRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Student>()), Times.Exactly(updateMockTimes));
            VerifyAddNotificationMock(addNotificationMockTimes);
        }

        private void RemoveProjectAsyncMockTimes(int updateRelationShipMockTimes, int addNotificationMockTimes)
        {
            _studentRepositoryMock.Verify(r => r.FindByIdAsync(It.IsAny<int>(), UtilTools.MockIIncludableQuery<Student>(), false), Times.Once());
            _studentRepositoryMock.Verify(r => r.UpdateRelationshipAsync(It.IsAny<Student>()), Times.Exactly(updateRelationShipMockTimes));
            VerifyAddNotificationMock(addNotificationMockTimes);
        }

        private void DeleteAsyncMockTimes(int deleteMockTimes, int addNotificationMockTimes)
        {
            _studentRepositoryMock.Verify(r => r.HaveObjectInDbAsync(It.IsAny<Expression<Func<Student, bool>>>()), Times.Once());
            _studentRepositoryMock.Verify(r => r.DeleteStudentAsync(It.IsAny<int>()), Times.Exactly(deleteMockTimes));
            VerifyAddNotificationMock(addNotificationMockTimes);
        }

        private void VerifyHaveStudentInDbAsyncMockTimes() =>
            _studentRepositoryMock.Verify(r => r.HaveObjectInDbAsync(It.IsAny<Expression<Func<Student, bool>>>()), Times.Once());
        
        private void VerifyAddNotificationMock(int addNotificationMockTimes) =>
            _notificationHandlerMock.Verify(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(addNotificationMockTimes));

        private void SetupAddNotificationInRange(int range)
        {
            for (var i = 0; i < range; i++)
            {
                _notificationHandlerMock.Setup(n => n.AddDomainNotification(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            }
        }

        private List<Student> BuildStudentListInRandomRange()
        {
            var randomRange = new Random().Next(0, 20);

            var studentList = new List<Student>();

            for (var i = 0; i < randomRange; i++)
            {
                var student = StudentBuilder.NewObject().DomainBuild();

                studentList.Add(student);
            }

            return studentList;
        }
    }
}
