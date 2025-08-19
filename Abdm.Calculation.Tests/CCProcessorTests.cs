using System.Numerics;
using System.Threading.Tasks;
using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.Models;
using Abdm.Calculation.Tests;
using Moq;
using Xunit;

public class CCProcessorTests
{
    [Theory]
    [MemberData(nameof(TestData))]
    public async Task Process_CompletesSuccessfully(CCRequestMessage message, CCResultMessage expectedResult)
    {
        // Arrange
        var mockMessage = new Mock<CCRequestMessage>();

        // Act
        var processor = new CCProcessor();
        await processor.Process(mockMessage.Object);

        // Assert
        var result = await processor.Process(mockMessage.Object);
        
        mockMessage.Verify(m => result.Allowed == expectedResult.Allowed 
        && result.PassType == expectedResult.PassType, Times.Once);
    }

    public static IEnumerable<object[]> TestData => CCProcessorTestData.TestData;
}