namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Address
{
    public sealed class AddressResponse
    {
        public required int Id { get; set; }
        public required string ZipCode { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string District { get; set; }
        public required string Number { get; set; }
        public string? Complement { get; set; }
    }
}
