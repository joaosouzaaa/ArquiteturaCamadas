using ArquiteturaCamadas.Api.Controllers;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using Moq;

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


    }
}
