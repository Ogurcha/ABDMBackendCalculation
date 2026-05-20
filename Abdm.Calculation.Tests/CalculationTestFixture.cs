using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.Coordinators;
using Abdm.Calculation.BLL.Mappers;
using Abdm.Calculation.DAL.DataTransferObjects;
using Abdm.Calculation.DAL.Entities;
using Abdm.Calculation.DAL.Enums;
using Abdm.Calculation.DAL.Interfaces;
using Abdm.Calculation.Infrastructure;
using Abdm.Calculation.WebApi.Infrastructure.MapsterConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Abdm.Calculation.Tests;

internal static class CalculationTestFixture
{
    internal static PassTypeCalculationCoordinator CreatePassTypeCoordinator(
        byte[] surfaceBinary,
        PassageIntervalDto[]? passageIntervals = null)
    {
        MapsterConfig.MapsterSetup();
        BLLMapsterConfig.BLLMapsterSetup();

        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["BLLSettings:UseLegacyLogic"] = "true",
                ["BLLSettings:UseSuperProfiles"] = "false"
            })
            .Build();

        services.AddSingleton<IConfiguration>(configuration);
        services.AddServices(configuration);

        ReplaceWithMock(services, CreatePassageIntervalRepository(passageIntervals ?? PassTypeCalculatorTestData.ResultFromPIRepo.Result));
        ReplaceWithMock(services, CreateSurfaceRepository(surfaceBinary));
        ReplaceWithMock(services, CreateSurfaceMaterialRepository());
        ReplaceWithMock(services, CreatePillarMaterialRepository());

        return services.BuildServiceProvider().GetRequiredService<PassTypeCalculationCoordinator>();
    }

    internal static byte[] LoadSurfaceBinary()
    {
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
            "Resources",
            "SurfaceDataExample");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"Surface binary fixture was not found at '{path}'. " +
                "Ensure Resources\\SurfaceDataExample is copied to the test output directory.");
        }

        return File.ReadAllBytes(path);
    }

    private static void ReplaceWithMock<T>(IServiceCollection services, T instance) where T : class
    {
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(T));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }

        services.AddSingleton(instance);
    }

    private static IPassageIntervalRepository CreatePassageIntervalRepository(PassageIntervalDto[] intervals)
    {
        var mock = new Mock<IPassageIntervalRepository>();
        mock.Setup(r => r.GetPassageIntervals(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(intervals);
        return mock.Object;
    }

    private static ISurfaceRepository CreateSurfaceRepository(byte[] surfaceBinary)
    {
        var mock = new Mock<ISurfaceRepository>();
        mock.Setup(r => r.GetSurfaceData(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SurfaceRawDataDto
            {
                data = surfaceBinary,
                c_cptype = (int)StrainCalculationTypeEnum.st14,
                c_typnk = (int)CheckPointTypeEnum.Surface,
                lambda = 12.5
            });
        return mock.Object;
    }

    private static ISurfaceMaterialRepository CreateSurfaceMaterialRepository()
    {
        var mock = new Mock<ISurfaceMaterialRepository>();
        mock.Setup(r => r.GetSurfaceMaterial(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SurfaceMaterialDto
            {
                c_mpsbm = 1,
                c_sistps = 1,
                c_typps = 1
            });
        return mock.Object;
    }

    private static IPillarMaterialRepository CreatePillarMaterialRepository()
    {
        var mock = new Mock<IPillarMaterialRepository>();
        mock.Setup(r => r.GetPillarMaterial(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PillarMaterialDto?)null);
        return mock.Object;
    }
}
