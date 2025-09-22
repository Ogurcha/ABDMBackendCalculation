using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainService
    {
        double GetStrain(PassTypeCalculationParameters message, SmoothPoints smoothpoints, double Y);
    }
}
