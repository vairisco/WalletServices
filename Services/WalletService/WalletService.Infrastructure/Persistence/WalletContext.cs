
using Microsoft.EntityFrameworkCore;
using WalletService.Domain.Entities;

namespace WalletService.Infrastructure.Persistence
{
    public partial class WalletContext : DbContext
    {
        public WalletContext(DbContextOptions<WalletContext> options) : base(options)
        {

        }

        public virtual DbSet<Product> Products { get; set; }
    }
}
