using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Project;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Student
{
    public sealed class StudentResponse : PersonResponse
    {
        public required ushort SchoolDivision { get; set; }

        public required List<ProjectResponse> Projects { get; set; }
    }
}
