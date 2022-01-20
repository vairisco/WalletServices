using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.API.Handler.ReCaptchaHandler;
using WalletService.Application.Contracts.Infrastructure;
using WalletService.Infrastructure.Recaptcha;

namespace WalletService.API.Extensions
{
    public static class ServiceReCaptchaRegistration
    {
        public static IServiceCollection AddReCaptchaRegistration(this IServiceCollection services)
        {
            services.AddHttpClient<IGoogleRecaptchaV3Service, GoogleRecaptchaV3Service>();
            services.AddTransient<IGoogleRecaptchaV3Service, GoogleRecaptchaV3Service>();
            services.AddScoped<IReCaptchaHandler, ReCaptchaHandler>();

            return services;
        }
    }
}
