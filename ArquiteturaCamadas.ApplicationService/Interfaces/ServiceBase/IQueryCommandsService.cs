namespace ArquiteturaCamadas.ApplicationService.Interfaces.ServiceBase
{
    public interface IQueryCommandsService<TResponse>
        where TResponse : class
    {
        Task<TResponse> FindByIdAsync(int id);
        Task<List<TResponse>> FindAllEntitiesAsync();
    }
}
