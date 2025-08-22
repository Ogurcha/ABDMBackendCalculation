using System.Threading.Tasks;
using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.Models;
using Moq;
using NUnit.Framework;

[TestFixture]
public class CCProcessorTests
{
    [Test]
    public async Task Process_CompletesSuccessfully()
    {
        var mockMessage = new Mock<CCRequestMessage>();

        var processor = new CCProcessor();
        await processor.Process(mockMessage.Object);

        mockMessage.Verify(m => processor.Process(m), Times.Once);
    }
}