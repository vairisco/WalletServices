using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Domain.Entities;

namespace WalletService.Application.Features.Products.Interface
{
    public interface IProductBusinessService
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
    }
}
