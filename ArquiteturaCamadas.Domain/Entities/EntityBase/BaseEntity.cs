namespace ArquiteturaCamadas.Domain.Entities.EntityBase
{
    public abstract class BaseEntity
    {
        public required int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
