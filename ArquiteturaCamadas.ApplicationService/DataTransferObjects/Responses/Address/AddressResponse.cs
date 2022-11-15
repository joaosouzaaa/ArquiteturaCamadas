namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Address
{
    public sealed class AddressResponse
    {
        public int Id { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Number { get; set; }
        public string? Complement { get; set; }
    }
}
