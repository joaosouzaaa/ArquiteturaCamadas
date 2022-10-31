using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Enums;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person
{
    public sealed class PersonUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public EGenderRequest Gender { get; set; }
    }
}
