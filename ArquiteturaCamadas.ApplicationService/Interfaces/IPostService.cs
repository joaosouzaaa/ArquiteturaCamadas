using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;
using ArquiteturaCamadas.ApplicationService.Interfaces.ServiceBase;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;

namespace ArquiteturaCamadas.ApplicationService.Interfaces
{
    public interface IPostService : ICommandsService<PostSaveRequest, PostUpdateRequest>
    {
        Task<PageList<PostTagsResponse>> FindAllEntitiesWithPaginationAsync(PageParams pageParams);
        Task<List<PostTagsResponse>> FindAllEntitiesAsync();
        Task<PostTagsResponse> FindByIdAsync(int id);
    }
}
