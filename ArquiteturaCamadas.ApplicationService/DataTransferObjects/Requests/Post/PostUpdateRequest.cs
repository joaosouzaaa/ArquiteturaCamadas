using Microsoft.AspNetCore.Http;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Post
{
    public sealed class PostUpdateRequest
    {
        public required int Id { get; set; }
        public required string Message { get; set; }
        public IFormFile? Image { get; set; }
        public List<int>? TagsIds { get; set; }
    }
}
