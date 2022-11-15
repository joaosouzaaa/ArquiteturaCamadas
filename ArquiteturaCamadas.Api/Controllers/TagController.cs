using ArquiteturaCamadas.Api.ControllersAttributes;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Tag;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArquiteturaCamadas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost("add-tag")]
        [CommandsResponseTypes]
        public async Task<bool> AddAsync([FromBody] TagSaveRequest tagSaveRequest) =>
            await _tagService.AddAsync(tagSaveRequest);

        [HttpPut("update-tag")]
        [CommandsResponseTypes]
        public async Task<bool> UpdateAsync([FromBody] TagUpdateRequest tagUpdateRequest) =>
            await _tagService.UpdateAsync(tagUpdateRequest);

        [HttpDelete("delete-tag")]
        [CommandsResponseTypes]
        public async Task<bool> DeleteAsync([FromQuery] int id) =>
            await _tagService.DeleteAsync(id);

        [HttpGet("find-tag")]
        [QueryCommandsResponseTypes]
        public async Task<TagResponse> FindByIdAsync([FromQuery] int id) =>
            await _tagService.FindByIdAsync(id);

        [HttpGet("find-all-tags")]
        [QueryCommandsResponseTypes]
        public async Task<List<TagPostsResponse>> FindAllEntitiesAsync() =>
            await _tagService.FindAllEntitiesAsync();

        [HttpGet("find-all-tags-paginated")]
        [QueryCommandsResponseTypes]
        public async Task<PageList<TagPostsResponse>> FindAllEntitiesWithPaginationAsync([FromQuery] PageParams pageParams) =>
            await _tagService.FindAllEntitiesWithPaginationAsync(pageParams);
    }
}
