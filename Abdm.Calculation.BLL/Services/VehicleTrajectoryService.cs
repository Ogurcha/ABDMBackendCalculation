using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Graphics.Models;
using g4;

namespace Abdm.Calculation.BLL.Services
{
    public class VehicleTrajectoryService(IMeshManager meshManager) : IVehicleTrajectoryService
    {
        /// <summary>
        /// Возвращает пересечение с поверхностью
        /// Вернёт null - если пересечения нет
        /// </summary>
        public ProfileYZ? GetProfileYZ(Mesh mesh, double X)
        {
            var profile = meshManager.GetIntersectionVectors(mesh, X);

            if (profile?.Any() != true)
            {
                return null;
            }

            var vectors = new SortedList<double, Vector3d>(
                profile.OrderBy(v => v.y)
                .Select((item) => new KeyValuePair<double, Vector3d>(item.y, item))
                .ToDictionary());

            return new ProfileYZ
            {
                X = X,
                Vectors = vectors
            };
        }

        public VehicleTrajectory[] GetVehicleTrajectories([DisallowNull] VehicleXPosition[] vehicleXPositions, Mesh mesh)
        {
            return vehicleXPositions
                .Select(x => GetVehicleTrajectory(x, mesh))
                .OfType<VehicleTrajectory>()
                .ToArray();
        }

        public VehicleTrajectory? GetVehicleTrajectory(VehicleXPosition xPosition, Mesh mesh)
        {
            var center = GetProfileYZ(mesh, xPosition.CenterXPosition);

            var left = xPosition.LeftXPosition
                .Select(x => GetProfileYZ(mesh, x))
                .OfType<ProfileYZ>()
                .ToArray();
            var right = xPosition.RightXPosition
                .Select(x => GetProfileYZ(mesh, x))
                .OfType<ProfileYZ>()
                .ToArray();

            if (center == null
                || left == null
                || right == null
                || xPosition.LeftXPosition.Length != left.Length
                || xPosition.RightXPosition.Length != right.Length)
            {
                return null;
            }

            return new VehicleTrajectory
            {
                Center = center,
                Left = left,
                Right = right
            };
        }
    }
}
