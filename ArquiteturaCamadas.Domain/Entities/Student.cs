using ArquiteturaCamadas.Domain.Enums;

namespace ArquiteturaCamadas.Domain.Entities
{
    public sealed class Student : Person
    {
        public ESchoolDivision SchoolDivision { get; set; } 

        public List<Project> Projects { get; set; }
    }
}
