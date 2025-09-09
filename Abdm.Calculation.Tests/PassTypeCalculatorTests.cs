using System.Collections.Generic;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.RoadRulesManager;
using Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy;
using Abdm.Calculation.BLL.Services;
using Abdm.Calculation.BLL.StrainCalculation;
using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.DAL;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Tests;
using Moq;
using NUnit.Framework;

[TestFixture]
public class PassTypeCalculatorTests
{
    Mock<IPassageIntervalRepository> _passageIntervalManagerMock;

    [SetUp]
    public void SetUp()
    {
        _passageIntervalManagerMock = new Mock<IPassageIntervalRepository>();
        _passageIntervalManagerMock.Setup(f => f.GetPassageIntervals(It.IsAny<long>()))
            .Returns(PassTypeCalculatorTestData.ResultFromPIRepo);
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

        var processor = new PassTypeCalculator(
            passageIntervalService, 
            new MeshManager(),
            roadRulesFactory,
            strainManager
            );

        try
        {
            var result = await processor.CalculatePassType(testMessage);

            Assert.That(result.PassType, Is.EqualTo(expectedOutput.PassType));
        }
        catch (System.Exception e)
        {
            Assert.Fail(e.Message);
        }
    }
}