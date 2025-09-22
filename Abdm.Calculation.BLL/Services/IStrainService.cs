using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services
{
    public interface IStrainService
    {
        double GetStrain(PTCRequestMessage message, SmoothPoints smoothpoints, double Y);
    }
}
