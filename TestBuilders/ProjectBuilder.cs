using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Project;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Project;

namespace TestBuilders
{
    public sealed class ProjectBuilder
    {
        private static Random _random = new Random();
        private DateTime _expiryDate = DateTime.Now;
        private string _name = "random";
        private int _studentId = _random.Next();
        private decimal _value = (decimal)_random.NextDouble();
        private int _id = _random.Next();

        public static ProjectBuilder NewObject() => new ProjectBuilder();

        public ProjectSaveRequest SaveRequestBuild() =>
            new ProjectSaveRequest()
            {
                ExpiryDate = _expiryDate,
                Name = _name,
                StudentId = _studentId,
                Value = _value
            };

        public ProjectUpdateRequest UpdateRequestBuild() =>
            new ProjectUpdateRequest()
            {
                ExpiryDate = _expiryDate,
                Id = _id,
                Name = _name,
                StudentId = _studentId,
                Value = _value
            };

        public ProjectResponse ResponseBuild() =>
            new ProjectResponse()
            {
                ExpiryDate = _expiryDate,
                Id = _id,
                Name = _name,
                Value = _value
            };
    }
}
