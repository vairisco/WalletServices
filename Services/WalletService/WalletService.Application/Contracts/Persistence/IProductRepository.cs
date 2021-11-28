
using WalletService.Domain.Entities;

namespace WalletService.Contracts.Persistence
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
        //Task<IEnumerable<Product>> GetOrdersByUserName(string userName);
    }
}
