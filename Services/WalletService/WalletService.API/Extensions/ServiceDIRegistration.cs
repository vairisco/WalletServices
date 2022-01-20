using Microsoft.Extensions.DependencyInjection;
using WalletService.Application.Contracts.Persistence;
using WalletService.Application.Features.Products;
using WalletService.Application.Features.Products.Interface;
using WalletService.Contracts.Persistence;
using WalletService.Infrastructure.Dapper.Persistence;
using WalletService.Infrastructure.Repositories;

namespace WalletService.API.Extensions
{
    public static class ServiceDIRegistration
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, Infrastructure.Dapper.Repositories.UnitOfWork>();
            //services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductRepository, Infrastructure.Dapper.Repositories.ProductRepository>();
            services.AddScoped<IProductBusinessService, ProductBusinessService>();

            return services;
        }
    }
}
