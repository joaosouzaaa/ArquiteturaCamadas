namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Project
{
    public sealed class ProjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
