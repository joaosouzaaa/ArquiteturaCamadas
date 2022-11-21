using ArquiteturaCamadas.Domain.Entities.EntityBase;

namespace ArquiteturaCamadas.Domain.Entities
{
    public sealed class Tag : BaseEntity
    {
        public required string TagName { get; set; }
        
        public List<Post> Posts { get; set; }
    }
}
