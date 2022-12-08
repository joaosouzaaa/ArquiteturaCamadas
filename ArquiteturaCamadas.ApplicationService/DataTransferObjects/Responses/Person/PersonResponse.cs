using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Address;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person
{
    public class PersonResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int Age { get; set; }
        public required ushort Gender { get; set; }
        public required AddressResponse Address { get; set; }
    }
}
