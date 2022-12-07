using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Project;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Student
{
    public sealed class StudentResponse : PersonResponse
    {
        public ushort SchoolDivision { get; set; }

        public List<ProjectResponse> Projects { get; set; }
    }
}
