using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.PassTypeCalculation
{
    public interface IPassTypeCalculator
    {
        Task<PTCResultMessage> CalculatePassType(PTCRequestMessage data);
    }
}
