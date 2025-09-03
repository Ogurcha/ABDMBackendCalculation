using System.Threading.Tasks;
using Abdm.Calculation.BLL.IntervalCalculation;
using Abdm.Calculation.BLL.RoadRules;
using Abdm.Calculation.BLL.StrainCalculation;
using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Tests;
using Moq;
using NUnit.Framework;

[TestFixture]
public class PassTypeCalculatorTests
{
    Mock<IPassageIntervalManager> _passageIntervalManagerMock;

    [SetUp]
    public void SetUp()
    {
        _passageIntervalManagerMock = new Mock<IPassageIntervalManager>();
        _passageIntervalManagerMock.Setup(f => f.GetPassageIntervals(It.IsAny<long>()))
            .Returns(PassTypeCalculatorTestData.ResultFromPIManager);
    }

    [Test]
    public async Task TestPassTypeCalculator()
    {
        var testMessage = PassTypeCalculatorTestData.TestRequestMessage;
        var expectedOutput = PassTypeCalculatorTestData.TestResultMessage;
        var roadRulesManager = new RoadRulesManager(new DataLifeSpanSettings { DataLifeSpanMinutes = 1 });
        var strainManager = new StrainManager();

        var processor = new PassTypeCalculator(
            _passageIntervalManagerMock.Object, 
            new MeshManager(),
            roadRulesManager,
            strainManager
            );

        var result = await processor.CalculatePassType(testMessage);

        Assert.That(result.PassType, Is.EqualTo(expectedOutput.PassType));
    }
}