using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IVehicleStrainProvider
    {
        VehicleStrain GetStrainOnTrajectory(VehicleTrajectory trajectory, double Y, LoadModel load, bool invertAxles);

        double GetZValueByY(ProfileYZ profile, double Y, out (Interval? i1, Interval? i2) positivePieces);
    }
}