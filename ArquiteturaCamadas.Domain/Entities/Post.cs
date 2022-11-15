using ArquiteturaCamadas.Domain.Entities.EntityBase;

namespace ArquiteturaCamadas.Domain.Entities
{
    public sealed class Post : BaseEntity
    {
        public string Message { get; set; }
        public byte[]? ImageBytes { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
