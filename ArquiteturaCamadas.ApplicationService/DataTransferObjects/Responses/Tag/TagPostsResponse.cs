using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag
{
    public sealed class TagPostsResponse
    {
        public required int Id { get; set; }
        public required string TagName { get; set; }
        public required List<PostResponse> Posts { get; set; }
    }
}
