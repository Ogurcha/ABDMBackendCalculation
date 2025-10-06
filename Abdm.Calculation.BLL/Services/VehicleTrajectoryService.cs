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
        /// По краям сразу зануляем профиль, чтобы ТС вышедшее краем за пределы не влияло на результат
        /// </summary>
        /// <param name="wheelLength"></param>
        public ProfileYZ? GetProfileYZ(Mesh mesh, 
            double X,
            double wheelLength)
        {
            var profile = meshManager.GetIntersectionVectors(mesh, X);

            if (profile?.Any() != true)
            {
                return null;
            }

            var firstVector = new Vector3d(profile.First().x, profile.First().y - wheelLength, 0); 
            var lastVector = new Vector3d(profile.Last().x, profile.Last().y + wheelLength, 0); 
            var vectors = new SortedList<double, Vector3d>(
                profile.Prepend(firstVector)
                .Append(lastVector)
                .OrderBy(v => v.y)
                .Select((item) => new KeyValuePair<double, Vector3d>(item.y, item))
                .ToDictionary());

            return new ProfileYZ
            {
                X = X,
                Vectors = vectors
            };
        }

        /// <summary>
        /// Получение траекторий движения ТС
        /// По краям сразу зануляем профили, чтобы ТС вышедшее частично за пределы проекций не влияло на результат
        /// </summary>
        /// <param name="wheelLength">Параметр для зануления краёв</param>
        /// <returns></returns>
        public VehicleTrajectory[] GetVehicleTrajectories([DisallowNull] VehicleXPosition[] vehicleXPositions, 
            Mesh mesh, 
            double wheelLength)
        {
            return vehicleXPositions
                .Select(x => GetVehicleTrajectory(x, mesh, wheelLength))
                .OfType<VehicleTrajectory>()
                .ToArray();
        }

        public VehicleTrajectory? GetVehicleTrajectory(VehicleXPosition xPosition, 
            Mesh mesh,
            double wheelLength)
        {
            var center = GetProfileYZ(mesh, xPosition.CenterXPosition, wheelLength);

            var left = xPosition.LeftXPosition
                .Select(x => GetProfileYZ(mesh, x, wheelLength))
                .OfType<ProfileYZ>()
                .ToArray();
            var right = xPosition.RightXPosition
                .Select(x => GetProfileYZ(mesh, x, wheelLength))
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
