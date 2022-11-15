namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Address
{
    public sealed class AddressRequest
    {
        public string ZipCode { get; set; }
        public string Number { get; set; }
        public string? Complement { get; set; }
    }
}
