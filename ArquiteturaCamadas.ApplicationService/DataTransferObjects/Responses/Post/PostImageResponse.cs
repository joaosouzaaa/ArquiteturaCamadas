namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post
{
    public sealed class PostImageResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public byte[]? ImageBytes { get; set; }
    }
}
