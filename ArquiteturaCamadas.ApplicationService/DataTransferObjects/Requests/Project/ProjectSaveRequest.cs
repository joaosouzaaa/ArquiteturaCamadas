namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Project
{
    public sealed class ProjectSaveRequest
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int StudentId { get; set; }
    }
}
