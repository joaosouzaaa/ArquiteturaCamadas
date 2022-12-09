using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Arguments;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Enums;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Address;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Student;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Address;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Project;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Student;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using Bogus;

namespace TestBuilders
{
    public sealed class StudentBuilder
    {
        private static readonly Faker _faker = new Faker();
        private Address _address = AddressBuilder.NewObject().DomainBuild();
        private int _addressId = GenerateRandomNumber();
        private int _age = GenerateRandomNumber();
        private EGender _gender = _faker.PickRandom<EGender>();
        private int _id = GenerateRandomNumber();
        private string _name = "random name";
        private List<Project> _projects = new List<Project>();
        private ESchoolDivision _schoolDivision = _faker.PickRandom<ESchoolDivision>();
        private AddressRequest _addressRequest = AddressBuilder.NewObject().RequestBuild();
        private EGenderRequest _genderRequest = _faker.PickRandom<EGenderRequest>();
        private ESchoolDivisionRequest _schoolDivisionRequest = _faker.PickRandom<ESchoolDivisionRequest>();
        private int _projectId = GenerateRandomNumber();
        private AddressResponse _addressResponse = AddressBuilder.NewObject().ResponseBuild();

        public static StudentBuilder NewObject() => new StudentBuilder();

        public Student DomainBuild() =>
            new Student()
            {
                Address = _address,
                AddressId = _addressId,
                Age = _age,
                Gender = _gender,
                Id = _id,
                Name = _name,
                Projects = _projects,
                SchoolDivision = _schoolDivision
            };

        public StudentSaveRequest SaveRequestBuild() =>
            new StudentSaveRequest()
            {
                Address = _addressRequest,
                Age = _age,
                Gender = _genderRequest,
                Name = _name,
                SchoolDivision = _schoolDivisionRequest
            };

        public StudentUpdateRequest UpdateRequestBuild() =>
            new StudentUpdateRequest()
            {
                Address = _addressRequest,
                Age = _age,
                Gender = _genderRequest,
                Id = _id,
                Name = _name,
                SchoolDivision = _schoolDivisionRequest
            };

        public ProjectStudentRelationShipArgument RelationShipArgumentBuild() =>
            new ProjectStudentRelationShipArgument()
            {
                ProjectId = _projectId,
                StudentId = _id
            };

        public StudentResponse ResponseBuild() =>
            new StudentResponse()
            {
                Address = _addressResponse,
                Age = _age,
                Gender = (ushort)_gender,
                Id = _id,
                Name = _name,
                Projects = new List<ProjectResponse>(),
                SchoolDivision = (ushort)_schoolDivision
            };

        private static int GenerateRandomNumber() =>
            new Random().Next();

        public StudentBuilder WithProjectId(int projectId)
        {
            _projectId = projectId;

            return this;
        }

        public StudentBuilder WithProjects(List<Project> projects)
        {
            _projects = projects;

            return this;
        }
    }
}
