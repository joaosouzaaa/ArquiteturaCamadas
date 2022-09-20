namespace ArquiteturaCamadas.ApplicationService.Interfaces.ServiceBase
{
    public interface ICommandsService<TSave, TUpdate>
        where TSave : class
        where TUpdate : class
    {
        Task<bool> AddAsync(TSave save);
        Task<bool> UpdateAsync(TUpdate update);
        Task<bool> DeleteAsync(int id);
    }
}
