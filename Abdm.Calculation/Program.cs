using Abdm.Calculation.Infrastructure;
using Abdm.Reports.Calculation.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Abdm.Reports.Calculation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging(cfg =>
            {
                cfg.ClearProviders();
                cfg.AddConfiguration(builder.Configuration.GetSection("Logging"));
#if DEBUG
                cfg.AddDebug();
#endif
                cfg.AddNLog();
            });

            builder.Services.AddServices(builder.Configuration);
            builder.Services.AddKafka(builder.Configuration);

            var app = builder.Build();

            app.Run();
        }
    }
}
