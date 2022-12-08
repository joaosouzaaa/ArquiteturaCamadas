using ArquiteturaCamadas.Domain.Entities.EntityBase;

namespace ArquiteturaCamadas.Domain.Entities
{
    public sealed class Project : BaseEntity
    {
        public required string Name { get; set; }
        public required decimal Value { get; set; }
        public required DateTime ExpiryDate { get; set; }

        public required int StudentId { get; set; }
        public required Student Student { get; set; }
    }
}
