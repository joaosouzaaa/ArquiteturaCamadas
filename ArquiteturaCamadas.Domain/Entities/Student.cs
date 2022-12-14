using ArquiteturaCamadas.Domain.Enums;

namespace ArquiteturaCamadas.Domain.Entities
{
    public sealed class Student : Person
    {
        public required ESchoolDivision SchoolDivision { get; set; } 

        public required List<Project> Projects { get; set; }
    }
}
