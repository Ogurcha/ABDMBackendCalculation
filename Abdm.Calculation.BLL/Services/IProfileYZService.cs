using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services
{
    public interface IProfileYZService
    {
        IEnumerable<Vector2D> GetYZFromProfile(ProfileYZ profile);

        double GetMaxZPosition(ProfileYZ profile);

        double GetStrain(ProfileYZ smoothpoints, double Y, LoadSchema loadSchema);
    }
}
