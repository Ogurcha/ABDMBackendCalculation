using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.RoadRulesManager;
using Abdm.Calculation.BLL.Services;
using Abdm.Calculation.BLL.Settings;
using Abdm.Calculation.BLL.StrainCalculation;
using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.DAL;
using Abdm.Calculation.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Abdm.Calculation.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
            services.Configure<DataLifeSpan>(configuration.GetSection("DataLifeSpan"));
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPassageIntervalRepository, PassageIntervalRepository>();
            services.AddScoped<IPassTypeCalculator, PassTypeCalculator>();
            
            services.AddSingleton<IStrainManager, StrainManager>();
            services.AddSingleton<IRoadRulesManager, RoadRulesManager>();
            services.AddSingleton<IPassageIntervalService, PassageIntervalService>();
        }
    }
}
