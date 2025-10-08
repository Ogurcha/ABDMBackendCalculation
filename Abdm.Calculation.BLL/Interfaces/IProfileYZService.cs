using Abdm.Calculation.BLL.Models.Parameters;
using Abdm.Calculation.BLL.Models.Primitives;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IProfileYZService
    {
        IEnumerable<Vector2D> GetYZFromProfile(ProfileYZ profile);

        double GetMaxZPosition(ProfileYZ profile);

        double GetStrain(ProfileYZ smoothpoints, double Y, LoadSchema loadSchema);
    }
}
