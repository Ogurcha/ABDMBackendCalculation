using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.PassTypeCalculation;
using Abdm.Calculation.BLL.RoadRulesManager;
using Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy;
using Abdm.Calculation.BLL.Services;
using Abdm.Calculation.BLL.StrainCalculation;
using Abdm.Calculation.DAL;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Tests;
using Abdm.Calculation.WebApi.Mappers;
using Abdm.Calculation.WebApi.RequestModels;
using Mapster;
using Moq;
using NUnit.Framework;

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
        _passageIntervalManagerMock = new Mock<IPassageIntervalRepository>();
        _passageIntervalManagerMock.Setup(f => f.GetPassageIntervals(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .Returns(PassTypeCalculatorTestData.ResultFromPIRepo);

        _surfaceDataRepositoryMock = new Mock<ISurfaceRepository>();

        using (ResourceReader reader = new ResourceReader(dataPath))
        {
            reader.GetResourceData(surfaceDataStr, out string resourceType, out byte[] resourceData);

            var csvData = Task.FromResult(resourceData.Skip(SurfaceDataExampleGarbageBytesCount).ToArray()) as Task<byte[]?>;

            _surfaceDataRepositoryMock.Setup(f => f.GetSurfaceData(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(csvData);
        }
    }

    [Test]
    public async Task TestPassTypeCalculator()
    {
        var testMessage = PassTypeCalculatorTestData.TestRequestMessage;
        var expectedOutput = PassTypeCalculatorTestData.TestResultMessage;
        var roadRulesFactory = new RoadRulesFactory(new List<BaseRRStrategy>() {
            new AbStrategy(),
            new AClassCommonStrategy(),
            new EN3Strategy(),
            new HeavyStrategy()
        });
        var strainManager = new StrainService();
        var passageIntervalService = new PassageIntervalService(_passageIntervalManagerMock.Object);
        var surfaceDataService = new SurfaceDataService(_surfaceDataRepositoryMock.Object);

        var processor = new PassTypeCalculationCoordinator(
            passageIntervalService,
            surfaceDataService,
            new MeshManager(),
            roadRulesFactory,
            strainManager
            );

        try
        {
            var result = await processor.GetPassType(testMessage, new System.Threading.CancellationToken());

            Assert.That(result.Data?.PassType, Is.EqualTo(expectedOutput.PassType));
        }
        catch (System.Exception e)
        {
            Assert.Fail(e.Message);
        }
    }
}