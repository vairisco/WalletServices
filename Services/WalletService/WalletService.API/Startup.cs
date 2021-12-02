using HealthChecks.UI.Client;
using Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using WalletService.API.Extensions;
using WalletService.API.Handler.RSAHandler;
using WalletService.API.RSAHandler;
using WalletService.Infrastructure.Persistence;

namespace WalletAPIService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        private readonly string _policyName = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRSAHandler, RSAHandler>();
            services.AddSerilog();
            services.AddDbContextPool<WalletContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Wallet_ConnectStringDb")));
            
            services.AddScoped<Func<WalletContext>>((provider) => () => provider.GetService<WalletContext>());
            services.AddScoped<DbFactory>();

            services.AddAutoMapper(typeof(Startup));

            services.AddHealthChecks();

            services.AddDIServices();
            services.AddServiceGrpcRegistration(Configuration);


            services.AddCors(opt =>
            {
                opt.AddPolicy(name: _policyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pay Service V2");
            });

            app.UseRouting();

            app.UseCors(_policyName);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ProductService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });

                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
