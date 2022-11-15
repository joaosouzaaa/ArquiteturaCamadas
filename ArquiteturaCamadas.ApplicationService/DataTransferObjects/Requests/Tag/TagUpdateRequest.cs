namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Tag
{
    public sealed class TagUpdateRequest
    {
        public int Id { get; set; }
        public string TagName { get; set; }
    }
}
