using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Graphics.Models;
using g4;

namespace Abdm.Calculation.BLL.GraphicsServices
{
    public class VehicleTrajectoryService(IMeshManager meshManager,
        IProfileYZService profileYZService) : IVehicleTrajectoryService
    {
        public IntervalModel GetIntervalModel(PassTypeSmallModel data, Mesh mesh, PassageInterval interval, RoadRule[] roadRules)
        {
            var result = new IntervalModel() { PassageIntervalRef = interval };

            var distinctXs = CalculateVehiclePositionsIncludingWheelOffsets(mesh.Data.DistinctXs, interval, data.Load, roadRules);
            result.Trajectories = GetVehicleTrajectories(distinctXs, mesh, data.Load.Axles);

            return result;
        }

        /// <summary>
        /// Возвращает пересечение с поверхностью
        /// Вернёт null - если пересечения нет
        /// По краям сразу зануляем профиль, чтобы ТС вышедшее краем за пределы не влияло на результат
        /// </summary>
        /// <param name="wheelLength">длина колеса нужна для зануления профиля</param>
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
        /// <param name="wheelLength">длина колеса нужна для зануления профиля по краям</param>
        /// <returns></returns>
        public VehicleTrajectory[] GetVehicleTrajectories([DisallowNull] VehicleXPosition[] vehicleXPositions, 
            Mesh mesh, 
            Axle[] axles)
        {
            var wheelLesngthAvg = axles.Select(x => x.Wx).Average();

            return vehicleXPositions
                .Select(x => GetVehicleTrajectory(x, mesh, wheelLesngthAvg))
                .OfType<VehicleTrajectory>()
                .ToArray();
        }

        public VehicleTrajectory? GetVehicleTrajectory(VehicleXPosition xPosition, 
            Mesh mesh,
            double wheelLength)
        {
            var center = GetProfileYZ(mesh, xPosition.CenterXPosition, wheelLength);
            if (center == null)
            {
                return null;
            }

            var left = new Dictionary<double, ProfileYZ>();
            foreach (var keyValuePair in xPosition.LeftXPosition)
            {
                var key = keyValuePair.Key;
                var value = GetProfileYZ(mesh, keyValuePair.Value, wheelLength);
                if (value == null)
                {
                    return null;
                }
                left.Add(key, value);
            }

            var right = new Dictionary<double, ProfileYZ>();
            foreach (var keyValuePair in xPosition.RightXPosition)
            {
                var key = keyValuePair.Key;
                var value = GetProfileYZ(mesh, keyValuePair.Value, wheelLength);
                if (value == null)
                {
                    return null;
                }
                right.Add(key, value);
            }

            return new VehicleTrajectory
            {
                Center = center,
                Left = left,
                Right = right
            };
        }

        /// <summary>
        /// Добирает координаты для проверок с учётом размера тележек
        /// </summary>
        /// <param name="distinctXs">Массив точек по оси Х для всей поверхности ИССО</param>
        /// <param name="passageInterval">Интервал проезда по оси Х, по которому должно проехать ТС</param>
        /// <returns>Массив точек по оси Х внутри данного интервала, и с учётом заездов и с учётом размера колёс</returns>
        public VehicleXPosition[] CalculateVehiclePositionsIncludingWheelOffsets(
            double[] distinctXs,
            PassageInterval passageInterval,
            LoadModel loadModel,
            RoadRule[] roadRules)
        {
            var result = new List<VehicleXPosition>();
            var safeDistance = Formulas.DistanceBetweenIntervalEdgeAndTrajectoryCenter(loadModel, roadRules);
            var wheelOffsetsMap = Formulas.DistanceBetweenTrajectoryCenterAndAxles(loadModel.Axles);

            var low = passageInterval.AbsolutePositionLeft + safeDistance;
            var high = passageInterval.AbsolutePositionRight - safeDistance;
            result.Add(new VehicleXPosition(low, wheelOffsetsMap.Keys));
            result.Add(new VehicleXPosition(high, wheelOffsetsMap.Keys));

            foreach (var x in distinctXs)
            {
                if (low < x && x < high)
                {
                    result.Add(new VehicleXPosition(x, wheelOffsetsMap.Keys));
                }
            }

            return result.OrderBy(x => x.CenterXPosition).ToArray();
        }

        /// <summary>
        /// Расчёт напряжения от ТС
        /// </summary>
        /// <param name="trajectory">Траектория по которой двигается ТС</param>
        /// <param name="Y">Точка, в которой считаем напряжение</param>
        /// <param name="load">параметры нагрузки</param>
        /// <returns></returns>
        public double GetStrainOnTrajectory(VehicleTrajectory trajectory, double Y, LoadModel load)
        {
            var strain = 0d;
            foreach (var axle in load.Axles)
            {
                var wheelWeight = axle.WheelWeight;
                foreach(var distance in axle.WheelsDistance)
                {
                    strain += profileYZService.GetStrain(trajectory.Left[distance], Y + axle.AbsolutePosition, wheelWeight)
                        + profileYZService.GetStrain(trajectory.Right[distance], Y + axle.AbsolutePosition, wheelWeight);
                }
            }
            return strain;
        }
    }
}
