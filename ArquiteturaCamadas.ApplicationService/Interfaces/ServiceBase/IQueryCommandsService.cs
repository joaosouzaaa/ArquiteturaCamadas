using ArquiteturaCamadas.Business.Settings.PaginationSettings;

namespace ArquiteturaCamadas.ApplicationService.Interfaces.ServiceBase
{
    public interface IQueryCommandsService<TResponse>
        where TResponse : class
    {
        Task<TResponse> FindByIdAsync(int id);
        Task<List<TResponse>> FindAllEntitiesAsync();
        Task<PageList<TResponse>> FindAllEntitiesWithPaginationAsync(PageParams pageParams);
    }
}
