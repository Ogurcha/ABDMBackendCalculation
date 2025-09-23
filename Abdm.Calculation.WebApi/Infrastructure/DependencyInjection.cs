using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.PassTypeCalculation;
using Abdm.Calculation.BLL.RoadRulesManager;
using Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy;
using Abdm.Calculation.BLL.Services;
using Abdm.Calculation.BLL.StrainCalculation;
using Abdm.Calculation.DAL;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Infrastructure.Settings;
using Abdm.Calculation.WebApi.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Abdm.Calculation.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IPassTypeModelsMapper, PassTypeModelsMapper>();
            services.AddScoped<IPassageIntervalRepository, PassageIntervalRepository>();
            services.AddScoped<ISurfaceRepository, SurfaceRepository>();
            services.AddScoped<IPassageIntervalService, PassageIntervalService>();
            services.AddScoped<ISurfaceDataService, SurfaceDataService>();
            services.AddSingleton<IMeshManager, MeshManager>();

            services.AddSingleton<IRoadRulesFactory, RoadRulesFactory>(x => new RoadRulesFactory(new System.Collections.Generic.List<BLL.RoadRulesManager.RoadRulesStrategy.BaseRRStrategy>
            {
                new AbStrategy(),
                new AClassCommonStrategy(),
                new EN3Strategy(),
                new HeavyStrategy()
            }));

            services.AddSingleton<IStrainService, StrainService>();
            services.AddScoped<IPassTypeCalculator, PassTypeCalculator>();
        }
    }
}
