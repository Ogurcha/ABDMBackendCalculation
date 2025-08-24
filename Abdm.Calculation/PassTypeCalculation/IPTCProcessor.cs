using System.Threading.Tasks;
using Abdm.Calculation.PassTypeCalculation.DTO;

namespace Abdm.Calculation.ColumnCalculation
{
    public interface IPTCProcessor
    {
        Task<PTCResultMessage> CalculatePassType(PTCRequestMessage data);
    }
}
