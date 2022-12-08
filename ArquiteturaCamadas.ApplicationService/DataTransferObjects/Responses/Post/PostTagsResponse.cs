using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post
{
    public sealed class PostTagsResponse
    {
        public required int Id { get; set; }
        public required string Message { get; set; }
        public byte[]? ImageBytes { get; set; }
        public required List<TagResponse> Tags { get; set; }
    }
}
