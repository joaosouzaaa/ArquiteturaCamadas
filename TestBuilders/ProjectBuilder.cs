using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Project;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Project;
using ArquiteturaCamadas.Domain.Entities;

namespace TestBuilders
{
    public sealed class ProjectBuilder
    {
        private static Random _random = new Random();
        private DateTime _expiryDate = DateTime.Now.AddDays(2);
        private string _name = "random";
        private int _studentId = _random.Next();
        private decimal _value = (decimal)_random.NextDouble();
        private int _id = _random.Next();
        public static ProjectBuilder NewObject() => new ProjectBuilder();

        public Project DomainBuild() =>
            new Project()
            {
                ExpiryDate = _expiryDate,
                Id = _id,
                Name = _name,
                Value = _value,
                Student = StudentBuilder.NewObject().DomainBuild(),
                StudentId = _studentId
            };

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

        public ProjectBuilder WithName(string name)
        {
            _name = name;

            return this;
        }

        public ProjectBuilder WithValue(decimal value)
        {
            _value = value;

            return this;
        }

        public ProjectBuilder WithExpiryDate(DateTime expiryDate)
        {
            _expiryDate = expiryDate;

            return this;
        }

        public ProjectBuilder WithId(int id)
        {
            _id = id;

            return this;
        }
    }
}
