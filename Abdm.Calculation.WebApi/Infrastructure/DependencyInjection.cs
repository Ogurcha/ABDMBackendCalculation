using System.Collections.Generic;
using Abdm.Calculation.BLL;
using Abdm.Calculation.BLL.GraphicsServices;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Mappers;
using Abdm.Calculation.BLL.Services;
using Abdm.Calculation.BLL.Services.PassTypes;
using Abdm.Calculation.BLL.Services.RoadRules;
using Abdm.Calculation.BLL.Services.RoadRules.Strategies;
using Abdm.Calculation.BLL.StrainCalculation;
using Abdm.Calculation.DAL;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Infrastructure.Settings;
using Abdm.Calculation.WebApi.Infrastructure.MapsterConfig;
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
            MapsterConfig.MapsterSetup();
            BLLMapsterConfig.BLLMapsterSetup();
            services.AddScoped<IPillarDataService, PillarDataService>();
            services.AddScoped<IEqualityComparer<double>, DoubleEqualityComparer>();
            services.AddScoped<IPassageIntervalRepository, PassageIntervalRepository>();
            services.AddScoped<ISurfaceRepository, SurfaceRepository>();
            services.AddScoped<IPassageIntervalService, PassageIntervalService>();
            services.AddScoped<ISurfaceDataService, SurfaceDataService>();
            services.AddScoped<IMeshManager, MeshManager>();
            services.AddScoped<IVehicleTrajectoryService, VehicleTrajectoryService>();

            services.AddSingleton<IRoadRulesFactory, RoadRulesFactory>(x => new RoadRulesFactory(new System.Collections.Generic.List<BaseRRStrategy>
            {
                new AbStrategy(),
                new AClassCommonStrategy(),
                new EN3Strategy(),
                new HeavyStrategy()
            }));

            services.AddScoped<IProfileYZService, ProfileYZService>();
            services.AddScoped<IVehiclePositioner, VehiclePositioner>();
            services.AddScoped<IPassTypeResolver, PassTypeResolver>();
            services.AddScoped<ITrajectorySelector, TrajectorySelector>();
            services.AddScoped<IStrainCalculator, StrainCalculator>();
            services.AddScoped<ICalculationCoordinator, CalculationCoordinator>();
            services.AddScoped<IPassTypeCalculationCoordinator, PassTypeCalculationCoordinator>();
            services.AddScoped<IPassTypeService, PassTypeService>();
        }
    }
}
