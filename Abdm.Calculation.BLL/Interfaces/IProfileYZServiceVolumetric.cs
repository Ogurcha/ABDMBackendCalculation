using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IProfileYZServiceVolumetric : IProfileYZService
    {
        ProfileYZExtended? GetProfileYZVolumetric(Mesh mesh,
            ProfileYZ profileYZ,
            IEnumerable<Axle> axleParams,
            double coatLength,
            Dictionary<double, ProfileYZ> profileMap);
    }
}