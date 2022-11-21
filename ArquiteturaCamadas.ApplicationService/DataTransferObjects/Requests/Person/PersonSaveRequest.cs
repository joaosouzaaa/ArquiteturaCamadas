using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Enums;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Address;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person
{
    public sealed class PersonSaveRequest
    {
        public required string Name { get; set; }
        public required int Age { get; set; }
        public required EGenderRequest Gender { get; set; }
        public required AddressRequest Address { get; set; }
    }
}
