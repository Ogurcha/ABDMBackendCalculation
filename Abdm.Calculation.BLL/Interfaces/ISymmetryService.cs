using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ISymmetryService
    {
        bool IsLoadSymmetric(LoadModel load);
    }
}