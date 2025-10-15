using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Graphics.Models;
using g4;

namespace Abdm.Calculation.BLL.GraphicsServices
{
    public class VehicleTrajectoryService(IMeshManager meshManager, IProfileYZService profileYZService) : IVehicleTrajectoryService
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
            var center = Get(xPosition.CenterXPosition);
            if (center == null) 
                return null;

            var left = Map(xPosition.LeftXPosition);
            if (left == null)
                return null;

            var right = Map(xPosition.RightXPosition);
            if (right == null)
                return null;

            return new VehicleTrajectory
            {
                Center = center,
                Left = left,
                Right = right
            };

            ProfileYZ? Get(double x) => GetProfileYZ(mesh, x, wheelLength);

            Dictionary<double, ProfileYZ>? Map(Dictionary<double, double> positions) =>
                positions
                    .Select(kv => (kv.Key, Value: Get(kv.Value)))
                    .All(p => p.Value != null)
                        ? positions.ToDictionary(kv => kv.Key, kv => Get(kv.Value)!)
                        : null;
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
            result.Add(GetXPostition(low));
            result.Add(GetXPostition(high));

            foreach (var x in distinctXs)
            {
                if (low < x && x < high)
                {
                    result.Add(GetXPostition(x));
                }
            }

            return result.OrderBy(x => x.CenterXPosition).ToArray();

            VehicleXPosition GetXPostition(double centerXPosition)
            {
                var left = new Dictionary<double, double>();
                var right = new Dictionary<double, double>();
                foreach (var halfWheelOffset in wheelOffsetsMap.Keys)
                {
                    left.Add(halfWheelOffset * 2, centerXPosition - halfWheelOffset);
                    right.Add(halfWheelOffset * 2, centerXPosition + halfWheelOffset);
                }
                return new VehicleXPosition()
                {
                    CenterXPosition = centerXPosition,
                    LeftXPosition = left,
                    RightXPosition = right,
                };
            }
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
            return load.Axles.Sum(axle => 
                axle.WheelsDistance.Sum(distance => 
                    profileYZService.GetStrain(trajectory.Left[distance], Y + axle.AbsolutePosition, axle.WheelWeight) 
                    + profileYZService.GetStrain(trajectory.Right[distance], Y + axle.AbsolutePosition, axle.WheelWeight)
                )
            );
        }
    }
}
