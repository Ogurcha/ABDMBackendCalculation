using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Entities;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainManager
    {
        double GetStrain(PTCRequestMessage message, SmoothPoints smoothpoints, double Y);
    }
}
