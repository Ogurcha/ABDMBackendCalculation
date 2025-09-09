using System.Threading.Tasks;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.ColumnCalculation
{
    public interface IPassTypeCalculator
    {
        Task<PTCResultMessage> CalculatePassType(PTCRequestMessage data);
    }
}
