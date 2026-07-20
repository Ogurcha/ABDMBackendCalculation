using System.Collections.Generic;
using Abdm.Calculation.BLL.Coordinators;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Mappers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Services;
using Abdm.Calculation.BLL.Services.LowLevelCalculation;
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
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.SteelConcrete;
using Abdm.Calculation.WebApi.Infrastructure.MapsterConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using odm16 = Abdm.Calculation.BLL.Services.StrainCoefficients.odm16;
using snip1938 = Abdm.Calculation.BLL.Services.StrainCoefficients.snip1938;
using snip1943 = Abdm.Calculation.BLL.Services.StrainCoefficients.snip1943;
using snip1948 = Abdm.Calculation.BLL.Services.StrainCoefficients.snip1948;
using snip1953 = Abdm.Calculation.BLL.Services.StrainCoefficients.snip1953;
using snip1962 = Abdm.Calculation.BLL.Services.StrainCoefficients.snip1962;
using snip1984 = Abdm.Calculation.BLL.Services.StrainCoefficients.snip1984;

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

            services.AddScoped<IVehicleStrainProvider, VehicleStrainProvider>();
            services.AddScoped<IVehicleStrainProvider, VehicleStrainProviderVolumetric>();
            services.AddScoped<IProfileYZService, ProfileYZService>();
            services.AddScoped<IProfileYZService, ProfileYZServiceVolumetric>();
            services.AddScoped<IProfileYZServiceVolumetric, ProfileYZServiceVolumetric>();
            services.AddScoped<IVehicleTrajectoryManager, VehicleTrajectoryManager>();
            services.AddScoped<IVehicleTrajectoryManager, VehicleTrajectoryManagerVolumetric>();

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
            services.AddSingleton<ICoefficientProviderFactory, CoefficientProviderFactory>(x 
                => new CoefficientProviderFactory(DependencyInjection.CoefficientProviderList()));
            services.AddSingleton<IPassTypeResolverFactory, PassTypeResolverFactory>(x => new PassTypeResolverFactory(new List<IPassTypeResolver>
            {
                new PassTypeResolver(),
                new SteelConcretePassTypeResolver(new SteelConcretePassChecker())
            }));
            services.AddSingleton<IStrainAnalyserFactory, StrainAnalyserFactory>(x => new StrainAnalyserFactory(new List<IAnalysisWriter>
            {
                new DefaultAnalysisWriter(),
                new SteelConcreteAnalysisWriter(),
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

        public static ICoefficientProvider[] CoefficientProviderList() => new ICoefficientProvider[]
        {
            new odm16.ABCoefficientProvider(),
            new odm16.AutoColumnCoefficientProvider(),
            new odm16.NClassCoefficientProvider(),
            new odm16.SingleCoefficientProvider(),
            new snip1938.SimpleCoefficientProvider(),
            new snip1938.TankCoefficientProvider(),
            new snip1943.SimpleCoefficientProvider(),
            new snip1943.TankCoefficientProvider(),
            new snip1948.SimpleCoefficientProvider(),
            new snip1948.TankCoefficientProvider(),
            new snip1953.SimpleCoefficientProvider(),
            new snip1962.MediumCoefficientProvider(),
            new snip1962.TankCoefficientProvider(),
            new snip1984.ABCoefficientProvider(),
            new snip1984.MediumCoefficientProvider(),
            new snip1984.TankCoefficientProvider(),
        };
           
    }
}
