using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.PassTypeCalculation.DTO;
using Abdm.Calculation.Tests;

public class CCProcessorTests
{
    [Theory]
    [MemberData(nameof(TestData))]
    public async Task Process_CompletesSuccessfully(PTCRequestMessage message, PTCResultMessage expectedResult)
    {
        var processor = new PTCProcessor();
        await processor.Process(message);

        // Assert
        var result = await processor.Process(message);

        Assert.True(result.Allowed == expectedResult.Allowed
        && result.PassType == expectedResult.PassType);
    }

    public static IEnumerable<object[]> TestData => CCProcessorTestData.TestData;
}