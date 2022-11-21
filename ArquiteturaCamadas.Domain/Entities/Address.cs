using ArquiteturaCamadas.Domain.Entities.EntityBase;

namespace ArquiteturaCamadas.Domain.Entities
{
    public sealed class Address : BaseEntity
    {
        public required string ZipCode { get; set; }
        public required string District { get; set; }
        public required string Number { get; set; }
        public required string? Complement { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
    }
}
