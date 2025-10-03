using System.Numerics;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services
{
    public interface IProfileYZService
    {
        IEnumerable<Vector2> GetFloatYZFromProfile(ProfileYZ profileVectors);
        double GetMaxZPosition(ProfileYZ profileVectors);
        double GetStrain(ProfileYZ smoothpoints, double Y, PassTypeCalculationParameters message);
    }
}
