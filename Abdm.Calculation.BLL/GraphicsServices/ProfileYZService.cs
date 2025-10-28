using System.Linq;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Extensions;
using Abdm.Calculation.Graphics.Models;
using Abdm.Calculation.Maths.Models;
using g4;

namespace Abdm.Calculation.BLL.StrainCalculation
{
    public class ProfileYZService(IEqualityComparer<double> equalityComparer) : IProfileYZService
    {
        /// <summary>
        /// Рассчет напряжения на профиле
        /// </summary>
        /// <param name="X"></param>
        public double GetStrain(ProfileYZ profile, double Y, double wheelWeight)
        {
            return wheelWeight * profile.GetZValueByY(Y);
        }

        public IEnumerable<Vector2D> GetYZFromProfile(ProfileYZ profile)
        {
            foreach (var v in profile.Vectors)
            {
                yield return new Vector2D(v.Value.y, v.Value.z);
            }
        }

        private SortedList<double, Vector3d> GetSuperProfileVectors(VehicleTrajectory vehicleTrajectory, PassTypeSmallModel data, bool invertAxles)
        {
            Func<Axle, double, double> axleFunc = invertAxles
            ? (axle, y) => { return y + data.Load.Length - axle.AbsolutePosition; }
            : (axle, y) => { return y + axle.AbsolutePosition; };

            var wheelsValues = data.Load.Axles.SelectMany(axle => axle.WheelsDistance.Select(wheelDistanceItem => (wheelDistanceItem, axle)));

            var distinctYs = vehicleTrajectory.Center.Vectors.Select(center => center.Value.y)
            .SelectMany(y => wheelsValues,
            (y, wheel) => axleFunc(wheel.axle, y)).Distinct(equalityComparer);

            return new SortedList<double, Vector3d>
                (distinctYs.Select(y =>
                    new Vector3d
                    {
                        x = vehicleTrajectory.X,
                        y = y,
                        z = wheelsValues.Sum(wheelValue =>
                            vehicleTrajectory.Left[wheelValue.wheelDistanceItem].GetZValueByY(axleFunc(wheelValue.axle, y))
                            * wheelValue.axle.WheelWeight
                            + vehicleTrajectory.Right[wheelValue.wheelDistanceItem].GetZValueByY(axleFunc(wheelValue.axle, y))
                            * wheelValue.axle.WheelWeight)
                    }).Select((item) => new KeyValuePair<double, Vector3d>(item.y, item))
                        .ToDictionary()
                );
        }
    }
}
