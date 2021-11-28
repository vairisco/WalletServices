using WalletService.Contracts.Persistence;
using WalletService.Domain.Entities;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        //public async Task<IEnumerable<Product>> GetOrdersByUserName(string userName)
        //{
        //    var orderList = await _dbContext.Products
        //                        .Where(o => o.UserName == userName)
        //                        .ToListAsync();
        //    return orderList;
        //}
    }
}
