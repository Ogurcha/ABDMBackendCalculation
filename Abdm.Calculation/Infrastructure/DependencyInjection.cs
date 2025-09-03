using Abdm.Calculation.BLL.IntervalCalculation;
using Abdm.Calculation.BLL.RoadRules;
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
            services.Configure<DataLifeSpanSettings>(configuration.GetSection("DataLifeSpanSettings"));
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPassageIntervalRepository, PassageIntervalRepository>();
            services.AddScoped<IPassTypeCalculator, PassTypeCalculator>();
            
            services.AddSingleton<IStrainManager, StrainManager>();
            services.AddSingleton<IRoadRulesManager, RoadRulesManager>();
            services.AddSingleton<IPassageIntervalManager, PassageIntervalManager>();
        }
    }
}
