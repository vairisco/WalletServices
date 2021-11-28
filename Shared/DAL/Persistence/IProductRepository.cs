using DAL.Persistence;
using Domains.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Persistence
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
        //Task<IEnumerable<Product>> GetOrdersByUserName(string userName);
    }
}
