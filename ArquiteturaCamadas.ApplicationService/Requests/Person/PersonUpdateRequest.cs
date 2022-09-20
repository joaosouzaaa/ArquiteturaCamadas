using ArquiteturaCamadas.ApplicationService.Requests.Enums;

namespace ArquiteturaCamadas.ApplicationService.Requests.Person
{
    public class PersonUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public EGenderRequest Gender { get; set; }
    }
}
