using ArquiteturaCamadas.Domain.Entities.EntityBase;

namespace ArquiteturaCamadas.Domain.Entities
{
    public sealed class Address : BaseEntity
    {
        public string ZipCode { get; set; }
        public string District { get; set; }
        public string Number { get; set; }
        public string? Complement { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
