namespace ArquiteturaCamadas.Domain.Entities.EntityBase
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
