using System;
using System.Threading.Tasks;
using WalletService.Application.Contracts.Persistence;
using WalletService.Infrastructure.Dapper.Persistence;

namespace WalletService.Infrastructure.Dapper.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbSession _session;
        public UnitOfWork(DbSession session)
        {
            _session = session;
        }

        public void BeginTransaction()
        {
            _session.Transaction = _session.Connection.BeginTransaction();
        }

        public Task<int> CommitAsync()
        {
            _session.Transaction.Commit();
            Dispose();
            return Task.FromResult(1);
        }

        private void Dispose() => _session.Transaction?.Dispose();

        public void Rollback()
        {
            _session.Transaction.Rollback();
            Dispose();
        }
    }
}
