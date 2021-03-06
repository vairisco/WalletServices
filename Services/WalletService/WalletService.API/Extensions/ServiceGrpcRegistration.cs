using Audit.Core;
using AuditLib;
using AuditLib.Grpc;
using DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace WalletService.API.Extensions
{
    public static class ServiceGrpcRegistration
    {
        public static IServiceCollection AddServiceGrpcRegistration(this IServiceCollection services, IConfiguration configuration)
        {

            var configAuditLog = configuration.GetSection(ConfigAuditLog.AuditLog).Get<ConfigAuditLog>();
            if (configAuditLog != null && configAuditLog.Enable)
            {
                services.ConfigureAudit(configAuditLog);
                services.AddScoped<AuditCore>();
                services.AddGrpc(options =>
                {
                    options.Interceptors.Add<AuditInterceptor>();
                });
            }
            else
                services.AddGrpc();

            services.AddGrpcHttpApi();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wallet Service", Version = "v1" });
            });
            services.AddGrpcSwagger();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
