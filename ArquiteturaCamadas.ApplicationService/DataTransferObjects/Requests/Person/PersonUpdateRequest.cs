using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Enums;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Address;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person
{
    public class PersonUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public EGenderRequest Gender { get; set; }
        public AddressRequest Address { get; set; }
    }
}
