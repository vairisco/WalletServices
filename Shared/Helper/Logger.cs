using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;

namespace Helper
{
    public static class Logger
    {
        public static IServiceCollection AddSerilog(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(config =>
            {
                // clear out default configuration
                config.ClearProviders();
                var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Fatal)
                .MinimumLevel.Override("Microsoft.AspNetCore.Server.Kestrel", LogEventLevel.Fatal)
                .MinimumLevel.Override("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogEventLevel.Fatal)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Fatal)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Infrastructure", LogEventLevel.Fatal)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Model.Validation", LogEventLevel.Fatal)
                .MinimumLevel.Override("Grpc", LogEventLevel.Warning)
                .WriteTo.Async(a => a.File(System.IO.Path.Combine($"{AppContext.BaseDirectory}LOG/", $"{System.Net.Dns.GetHostName()}-log.txt"),
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        fileSizeLimitBytes: 209715200, //~200 mb
                        buffered: true,
                        retainedFileCountLimit: null,
                        flushToDiskInterval: TimeSpan.FromSeconds(5),
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{RequestId}] [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    ), bufferSize: 1000).CreateLogger();
                config.AddSerilog(logger, true);
                Log.Logger = logger;
            });
            return serviceCollection;
        }
    }
}
