using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Tag;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Tag;
using ArquiteturaCamadas.ApplicationService.Interfaces.ServiceBase;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;

namespace ArquiteturaCamadas.ApplicationService.Interfaces
{
    public interface ITagService :
                     ICommandsService<TagSaveRequest, TagUpdateRequest>
    {
        Task<TagResponse> FindByIdAsync(int id);
        Task<PageList<TagPostsResponse>> FindAllEntitiesWithPaginationAsync(PageParams pageParams);
        Task<Tag> FindByIdAsyncAsNoTrackingReturnsDomainObject(int id);
        Task<List<TagPostsResponse>> FindAllEntitiesAsync();
    }
}
