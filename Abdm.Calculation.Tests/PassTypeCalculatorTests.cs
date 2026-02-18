using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Abdm.Calculation.BLL;
using Abdm.Calculation.BLL.GraphicsServices;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Mappers;
using Abdm.Calculation.BLL.Coordinators;
using Abdm.Calculation.BLL.Coordinators.RoadRules;
using Abdm.Calculation.BLL.Coordinators.RoadRules.Strategies;
using Abdm.Calculation.BLL.StrainCalculation;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Tests;
using Abdm.Calculation.WebApi.Infrastructure.MapsterConfig;
using Moq;
using NUnit.Framework;
using Abdm.Calculation.DAL.DataTransferObjects;
using Abdm.Calculation.DAL.Interfaces;

[TestFixture]
public class PassTypeCalculatorTests
{
    private const string surfaceDataStr = "SurfaceDataExample";
    private const int SurfaceDataExampleGarbageBytesCount = 4;
    private const string resourcesStr = "Resources";
    private readonly string dataPath = Path.Combine(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
        resourcesStr,
        surfaceDataStr
        );

    Mock<IPassageIntervalRepository> _passageIntervalManagerMock;
    Mock<ISurfaceRepository> _surfaceDataRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        MapsterConfig.MapsterSetup();
        BLLMapsterConfig.BLLMapsterSetup();
        _passageIntervalManagerMock = new Mock<IPassageIntervalRepository>();
        _passageIntervalManagerMock.Setup(f => f.GetPassageIntervals(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .Returns(PassTypeCalculatorTestData.ResultFromPIRepo);

        _surfaceDataRepositoryMock = new Mock<ISurfaceRepository>();

        using (ResourceReader reader = new ResourceReader(dataPath))
        {
            reader.GetResourceData(surfaceDataStr, out string resourceType, out byte[] resourceData);

            var csvData = resourceData.Skip(SurfaceDataExampleGarbageBytesCount).ToArray();

            _surfaceDataRepositoryMock.Setup(f => f.GetSurfaceData(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult((SurfaceRawDataDto?)new SurfaceRawDataDto { data = csvData }));
        }
    }

}