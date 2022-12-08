using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Enums;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Address;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person
{
    public class PersonUpdateRequest
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int Age { get; set; }
        public required EGenderRequest Gender { get; set; }
        public required AddressRequest Address { get; set; }
    }
}
