using System.Threading.Tasks;
using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.Tests;
using NUnit.Framework;

[TestFixture]
public class PTCProcessorTests
{
    [SetUp]
    public void SetUp()
    {

    }


    [Test]
    public async Task Process_CompletesSuccessfully()
    {
        var testMessage = PTCProcessorTestData.TestRequestMessage;
        var expectedOutput = PTCProcessorTestData.TestResultMessage;

        var processor = new PTCProcessor();
        var result = await processor.CalculatePassType(testMessage);

        Assert.That(result.PassType == expectedOutput.PassType);
    }
}