using ArquiteturaCamadas.Domain.Entities.EntityBase;

namespace ArquiteturaCamadas.Domain.Entities
{
    public sealed class Project : BaseEntity
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime ExpiryDate { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
