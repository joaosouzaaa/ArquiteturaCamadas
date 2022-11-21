using ArquiteturaCamadas.Domain.Entities.EntityBase;
using ArquiteturaCamadas.Domain.Enums;

namespace ArquiteturaCamadas.Domain.Entities
{
    public class Person : BaseEntity
    {
        public required string Name { get; set; }
        public required int Age { get; set; }
        public required EGender Gender { get; set; }

        public required int AddressId { get; set; }
        public required Address Address { get; set; }
    }
}
