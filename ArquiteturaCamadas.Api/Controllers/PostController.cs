using ArquiteturaCamadas.Api.ControllersAttributes;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArquiteturaCamadas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("add-post")]
        [CommandsResponseTypes]
        public async Task<bool> AddAsync([FromForm] PostSaveRequest postSaveRequest) =>
            await _postService.AddAsync(postSaveRequest);

        [HttpPut("update-post")]
        [CommandsResponseTypes]
        public async Task<bool> UpdateAsync([FromForm] PostUpdateRequest postUpdateRequest) =>
            await _postService.UpdateAsync(postUpdateRequest);

        [HttpDelete("delete-post")]
        [CommandsResponseTypes]
        public async Task<bool> DeleteAsync([FromQuery] int id) =>
            await _postService.DeleteAsync(id);

        [HttpGet("find-all-posts-paginated")]
        [QueryCommandsResponseTypes]
        public async Task<PageList<PostTagsResponse>> FindAllEntitiesWithPaginationAsync([FromQuery] PageParams pageParams) =>
            await _postService.FindAllEntitiesWithPaginationAsync(pageParams);

        [HttpGet("find-all-posts")]
        [QueryCommandsResponseTypes]
        public async Task<List<PostTagsResponse>> FindAllEntitiesAsync() =>
            await _postService.FindAllEntitiesAsync();

        [HttpGet("find-post")]
        public async Task<PostTagsResponse> FindByIdAsync([FromQuery] int id) =>
            await _postService.FindByIdAsync(id);
    }
}
