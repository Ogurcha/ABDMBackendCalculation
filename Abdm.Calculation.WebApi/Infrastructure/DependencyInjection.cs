using System.Collections.Generic;
using Abdm.Calculation.BLL.Coordinators;
using Abdm.Calculation.BLL.GraphicsServices;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Mappers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Services;
using Abdm.Calculation.BLL.Services.PassTypes;
using Abdm.Calculation.BLL.Services.RoadRules;
using Abdm.Calculation.BLL.Services.RoadRules.Strategies;
using Abdm.Calculation.BLL.Services.StrainAnlysis;
using Abdm.Calculation.BLL.Services.StrainAnlysis.Strategies;
using Abdm.Calculation.BLL.Services.StrainCoefficients;
using Abdm.Calculation.BLL.Services.SurfaceData;
using Abdm.Calculation.BLL.Services.SurfaceData.Parsers;
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
            services.AddScoped<ITrajectoryFilterProvider, TrajectoryFilterProvider>();
            services.AddScoped<IPassageIntervalService, PassageIntervalService>();
            services.AddScoped<ISurfaceDataService, SurfaceDataService>();
            services.AddScoped<IMeshManager, MeshManager>();
            services.AddScoped<ISymmetryService, SymmetryService>();
            services.AddScoped<IVehicleTrajectoryService, VehicleTrajectoryService>();
            services.AddScoped<ISteelConcretePassChecker, SteelConcretePassChecker>();
            services.AddScoped<IStrainAnalyser, StrainAnalyser>();

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
            services.AddSingleton<IStrainCoefficientFactory, StrainCoefficientFactory>(x => new StrainCoefficientFactory(new List<ICoefficientCalculator>
            {
                new BasicStrainCoefficientCalculator(),
                new DynamicMovementCoefficientCalculator(),
                new DynamicMovementPillarCoefficientCalculator(),
                new TrafficJamStrainCoefficientCalculator(),
            }));
            services.AddSingleton<IPassTypeResolverFactory, PassTypeResolverFactory>(x => new PassTypeResolverFactory(new List<IPassTypeResolver>
            {
                new PassTypeResolver(x.GetRequiredService<IStrainCoefficientFactory>()),
                new SteelConcretePassTypeResolver(x.GetRequiredService<IStrainCoefficientFactory>(), new SteelConcretePassChecker())
            }));
            services.AddSingleton<IStrainAnalyserFactory, StrainAnalyserFactory>(x => new StrainAnalyserFactory(new List<ISAStrategy>
            {
                new DefaultStrainAnalyser(),
                new SteelConcreteAnalyser(),
            }));


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
            services.AddScoped<IStrainResultPopulator, StrainResultPopulator>();
            services.AddScoped<IStrainResultService, StrainResultService>();
            services.AddScoped<IBaseVehicleRollingCalculationCoordinator, BaseVehicleRollingCalculationCoordinator>();

           
            services.AddWorker<PassTypeCalculationCoordinator, PassTypeCalculationParameters, PassTypeCalculationResult>();
            services.AddWorker<StrainAnalysisCalculationCoordinator, StrainAnalysisParameters, StrainAnalysisResult>();
            
        }

        public static void AddWorker<T, Param, Result>(this IServiceCollection services) where T : class, ICoordinator<Param, Result> where Result : class where Param : class
        {
            services.AddScoped<T>();
            services.AddScoped<ICanWork<Param, Result>, WorkerWrapper<T, Param, Result>>();
        }
    }
}
