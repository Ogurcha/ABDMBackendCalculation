using System.Collections.Generic;
using Abdm.Calculation.BLL;
using Abdm.Calculation.BLL.GraphicsServices;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Mappers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Services;
using Abdm.Calculation.BLL.Services.PassTypes;
using Abdm.Calculation.BLL.Services.RoadRules;
using Abdm.Calculation.BLL.Services.RoadRules.Strategies;
using Abdm.Calculation.BLL.Services.StrainCoefficients;
using Abdm.Calculation.BLL.Services.SurfaceData;
using Abdm.Calculation.BLL.Services.SurfaceData.Parsers;
using Abdm.Calculation.BLL.StrainCalculation;
using Abdm.Calculation.DAL;
using Abdm.Calculation.DAL.Interfaces;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Infrastructure.Settings;
using Abdm.Calculation.SteelConcrete;
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
            services.Configure<BLLSettings>(configuration.GetSection("BLLSettings"));
            services.AddScoped<IEqualityComparer<double>, DoubleEqualityComparer>();
            services.AddScoped<IPassageIntervalRepository, PassageIntervalRepository>();
            services.AddScoped<ISurfaceRepository, SurfaceRepository>();
            services.AddScoped<ISurfaceMaterialRepository, SurfaceMaterialRepository>();
            services.AddScoped<IPillarMaterialRepository, PillarMaterialRepository>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<IPassageIntervalService, PassageIntervalService>();
            services.AddScoped<ISurfaceDataService, SurfaceDataService>();
            services.AddScoped<IMeshManager, MeshManager>();
            services.AddScoped<IVehicleTrajectoryService, VehicleTrajectoryService>();
            services.AddScoped<ISymmetryService, SymmetryService>();
            services.AddScoped<ISteelConcretePassChecker, SteelConcretePassChecker>();

            services.AddSingleton<IRoadRulesFactory, RoadRulesFactory>(x => new RoadRulesFactory(new System.Collections.Generic.List<BaseRRStrategy>
            {
                new AbStrategy(),
                new CommonStrategy(),
                new HeavyStrategy(),
                new VehicleColumnStrategy(),
            }));
            services.AddSingleton<ISurfaceBinaryParserFactory, SurfaceBinaryParserFactory>(x => new SurfaceBinaryParserFactory(new List<ISurfaceBinaryParser>
            {
                new BaseSurfaceBinaryParser(),
                new PillarSurfaceBinaryParser(new PillarDataService()),
                new SteelConcreteSurfaceBinaryParser()
            }));
            services.AddSingleton<IPassTypeResolverFactory, PassTypeResolverFactory>(x => new PassTypeResolverFactory(new List<IPassTypeResolver>
            {
                new PassTypeResolver(),
                new SteelConcretePassTypeResolver(new SteelConcretePassChecker())
            }));
            services.AddSingleton<IStrainCoefficientFactory, StrainCoefficientFactory>(x => new StrainCoefficientFactory(new List<ICoefficientCalculator>
            {
                new BasicStrainCoefficientCalculator(),
                new DynamicMovementCoefficientCalculator(),
                new DynamicMovementPillarCoefficientCalculator(),
                new TrafficJamStrainCoefficientCalculator(),
            }));

            services.AddScoped<IProfileYZService, ProfileYZService>();
            if (configuration.GetSection("BLLSettings").GetSection("UseLegacyLogic").Value == true.ToString())
            {
                services.AddScoped<IVehiclePositioner, AxleVehiclePositioner>();
            }
            else
            {
                services.AddScoped<IVehiclePositioner, IterationVehiclePositioner>();
            }
            

            services.AddScoped<IStrainCalculator, StrainCalculator>();
            services.AddScoped<IStrainSelector, StrainSelector>();
            services.AddScoped<IStrainResultService, StrainResultService>();
            services.AddScoped<IPassTypeCalculationCoordinator, PassTypeCalculationCoordinator>();
            services.AddScoped<IPassTypeService, PassTypeService>();
        }
    }
}
