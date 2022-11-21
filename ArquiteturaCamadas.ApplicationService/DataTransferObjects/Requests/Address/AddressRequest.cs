namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Address
{
    public sealed class AddressRequest
    {
        public required string ZipCode { get; set; }
        public required string Number { get; set; }
        public string? Complement { get; set; }
    }
}
