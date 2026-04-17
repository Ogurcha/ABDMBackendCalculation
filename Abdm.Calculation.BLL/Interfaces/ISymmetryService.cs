using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ISymmetryService
    {
        bool[] CalculateDirection(bool? isSymmetric, Enums.DriveDirectionEnum directionEnum);
        bool IsLoadSymmetric(LoadModel load);
    }
}