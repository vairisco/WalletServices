using System;
using System.Threading.Tasks;
using WalletService.Application.Contracts.Persistence;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbFactory _dbFactory;
        public UnitOfWork(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public Task<int> CommitAsync()
        {
            return _dbFactory.DbContext.SaveChangesAsync();
        }
    }
}
