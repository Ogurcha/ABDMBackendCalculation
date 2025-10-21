using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IProfileYZService
    {
        IEnumerable<Vector2D> GetYZFromProfile(ProfileYZ profile);

        double GetStrain(ProfileYZ profile, double Y, double wheelWeight);
    }
}
