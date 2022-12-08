namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Project
{
    public sealed class ProjectResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Value { get; set; }
        public required DateTime ExpiryDate { get; set; }
    }
}
