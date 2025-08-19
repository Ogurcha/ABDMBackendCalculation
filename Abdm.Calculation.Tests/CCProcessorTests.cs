using System.Threading.Tasks;
using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.Models;
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
        mockMessage.Verify(m => processor.Process(m), Times.Once);
    }

    public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            //1
            new object[] {
                new CCRequestMessage()
                {
                    C_isso = 38000331,
                    NagruzkaId = [20, 40, 170],
                    Snip = ais7PcSnip.odm16,
                    Direction = ais7DriveDirection.Bidirection,
                },
                new CCResultMessage()
                {
                    IssoId = 38000331,
                    PassTypes = new CheckpointPassType[]
                    {
                        new CheckpointPassType
                        {
                             CheckpointNumber = 1,
                             NagruzkaId = 20,
                             PassType = 1,
                        }
                    }
                }
            },
            //2
            new object[] {
                new CCRequestMessage()
                {

                },
                ais7PassTypeEnum.NoLimit
            }
            
        };

}