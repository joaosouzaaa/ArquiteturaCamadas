using ArquiteturaCamadas.Business.Interfaces.OtherInterfaces;
using ArquiteturaCamadas.Infra.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ArquiteturaCamadas.Infra.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseFacade _databaseFacade;

        public UnitOfWork(ArquiteturaCamadasDbContext dbContext)
        {
            _databaseFacade = dbContext.Database;
        }

        public void CommitTransaction()
        {
            try
            {
                _databaseFacade.CommitTransaction();
            }
            catch
            {
                _databaseFacade.RollbackTransaction();

                throw;
            }
        }

        public void RollbackTransaction() => _databaseFacade.RollbackTransaction();

        public void BeginTransaction() => _databaseFacade.BeginTransaction();   
    }
}
