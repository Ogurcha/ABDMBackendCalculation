using Abdm.Calculation.Graphics;
using Abdm.Calculation.PassTypeCalculation.DTO;

namespace Abdm.Calculation.StrainCalculation
{
    public interface IStrainManager
    {
        double GetStrain(PTCRequestMessage message, SmoothPoints smoothpoints, double Y);
    }
}