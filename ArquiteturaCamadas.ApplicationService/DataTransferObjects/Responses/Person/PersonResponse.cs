namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Person
{
    public sealed class PersonResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public ushort Gender { get; set; }
    }
}
