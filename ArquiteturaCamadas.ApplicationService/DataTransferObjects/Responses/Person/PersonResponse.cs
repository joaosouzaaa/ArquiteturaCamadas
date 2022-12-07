using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Address;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person
{
    public class PersonResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public ushort Gender { get; set; }
        public AddressResponse Address { get; set; }
    }
}
