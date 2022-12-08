namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Tag
{
    public sealed class TagUpdateRequest
    {
        public required int Id { get; set; }
        public required string TagName { get; set; }
    }
}
