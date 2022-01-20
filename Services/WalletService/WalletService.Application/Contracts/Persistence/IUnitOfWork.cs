using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletService.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        Task<int> CommitAsync();
        void Rollback();
    }
}
