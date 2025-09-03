using System.Threading.Tasks;
using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.G4;
using Abdm.Calculation.IntervalCalculation;
using Abdm.Calculation.RoadRules;
using Abdm.Calculation.StrainCalculation;
using Abdm.Calculation.Tests;
using Moq;
using NUnit.Framework;

[TestFixture]
public class PassTypeCalculatorTests
{
    [SetUp]
    public void SetUp()
    {

    }

    [Test]
    public async Task TestPassTypeCalculator()
    {
        var testMessage = PassTypeCalculatorTestData.TestRequestMessage;
        var expectedOutput = PassTypeCalculatorTestData.TestResultMessage;
        var repo = new Mock<IPassageIntervalManager>();
        var roadRulesManager = new Mock<IRoadRulesManager>();
        var strainManager = new Mock<IStrainManager>();

        var processor = new PassTypeCalculator(
            repo.Object, 
            new MeshProcessor(),
            roadRulesManager.Object,
            strainManager.Object
            );
        var result = await processor.CalculatePassType(testMessage);

        Assert.That(result.PassType == expectedOutput.PassType);
    }
}