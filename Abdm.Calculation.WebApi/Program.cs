using Abdm.Calculation.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Abdm.Reports.Calculation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSettings(builder.Configuration);
            builder.Services.AddServices(builder.Configuration);
            builder.Services.AddKafka(builder.Configuration);

            builder.Services.AddLogging(cfg =>
            {
                cfg.ClearProviders();
                cfg.AddNLog();
                cfg.SetMinimumLevel(LogLevel.Information);
                cfg.AddConfiguration(builder.Configuration.GetSection("Logging"));
#if DEBUG
                cfg.AddDebug();
#endif
                cfg.AddConsole();
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
