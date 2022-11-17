using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ArquiteturaCamadas.Business.Interfaces.OtherInterfaces
{
    public interface IUnitOfWork
    {
        void CommitTransaction();
        void RollbackTransaction();
        void BeginTransaction();
    }
}
