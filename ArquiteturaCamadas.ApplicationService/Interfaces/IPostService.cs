using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Post;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Post;
using ArquiteturaCamadas.ApplicationService.Interfaces.ServiceBase;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;

namespace ArquiteturaCamadas.ApplicationService.Interfaces
{
    public interface IPostService
    {
        Task<bool> AddAsync(PostSaveRequest postSaveRequest);
        Task<bool> UpdateUnreapeatTagsSearchAsync(PostUpdateRequest postUpdateRequest);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateManyToManyAsync(PostUpdateRequest postUpdateRequest);
        Task<PageList<PostTagsResponse>> FindAllEntitiesWithPaginationAsync(PageParams pageParams);
        Task<List<PostTagsResponse>> FindAllEntitiesAsync();
        Task<PostTagsResponse> FindByIdAsync(int id);
    }
}
