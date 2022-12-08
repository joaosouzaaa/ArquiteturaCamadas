namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Project
{
    public sealed class ProjectUpdateRequest
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Value { get; set; }
        public required DateTime ExpiryDate { get; set; }
        public required int StudentId { get; set; }
    }
}
