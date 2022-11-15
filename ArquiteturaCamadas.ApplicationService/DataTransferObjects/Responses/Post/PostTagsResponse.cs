using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post
{
    public sealed class PostTagsResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public byte[]? ImageBytes { get; set; }
        public List<TagResponse> Tags { get; set; }
    }
}
