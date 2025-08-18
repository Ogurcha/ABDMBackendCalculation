using System.Threading.Tasks;
using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.Models;
using Moq;
using Xunit;

public class CCProcessorTests
{
    [Fact]
    public async Task Process_CompletesSuccessfully()
    {
        // Arrange
        var mockMessage = new Mock<CCRequestMessage>();

        // Act
        var processor = new CCProcessor();
        await processor.Process(mockMessage.Object);

        // Assert
        mockMessage.Verify(m => processor.Process(m), Times.Once);
    }

    [Fact]
    public async Task Process_ThrowsException_WhenMessageIsNull()
    {
        // Arrange
        var processor = new CCProcessor();

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            processor.Process(null));
    }
}