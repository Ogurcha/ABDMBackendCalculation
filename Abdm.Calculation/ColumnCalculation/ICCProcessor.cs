using System.Threading.Tasks;
using Abdm.Calculation.Models;

namespace Abdm.Calculation.ColumnCalculation
{
    public interface ICCProcessor
    {
        Task<CCResultMessage> Process(CCRequestMessage data);
    }
}
