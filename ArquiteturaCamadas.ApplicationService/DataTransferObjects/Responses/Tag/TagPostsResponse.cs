using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag
{
    public sealed class TagPostsResponse
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public List<PostResponse> Posts { get; set; }
    }
}
