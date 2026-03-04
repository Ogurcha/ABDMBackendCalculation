using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Graphics.Models;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;
using g4;


namespace Abdm.Calculation.BLL.GraphicsServices
{
    public class VehicleTrajectoryService(
        IMeshManager meshManager,
        IProfileYZService profileYZService,
        ITrajectoryFilterProvider trajectoryFilterProvider) : IVehicleTrajectoryService
    {

        public IntervalModel GetIntervalModel(
            VehicleRollingBigModel dataModel, 
            PassageInterval interval)
        {
            var result = new IntervalModel() { PassageIntervalRef = interval };

            var distinctXs = CalculateVehiclePositionsIncludingWheelOffsets(
                dataModel.Mesh.Data.DistinctXs, 
                interval, 
                dataModel.Data.Load,
                dataModel.RoadRules);

            result.Trajectories = GetVehicleTrajectories(
                distinctXs, 
                dataModel.Mesh, 
                dataModel.Data.Load.Axles);

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

            var sorted = profile.OrderBy(v => v.y);
            var firstVector = new Vector3d(sorted.First().x, sorted.First().y - wheelLength, 0); 
            var lastVector = new Vector3d(sorted.Last().x, sorted.Last().y + wheelLength, 0); 
            var vectors = new SortedList<double, Vector3d>(
                sorted.Prepend(firstVector)
                .Append(lastVector)
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
            var wheelLengthAvg = axles.Select(x => x.Wx).Average();

            return vehicleXPositions
                .Select(x => GetVehicleTrajectory(x, mesh, wheelLengthAvg))
                .OfType<VehicleTrajectory>()
                .ToArray();
        }

        public VehicleTrajectory? GetVehicleTrajectory(VehicleXPosition xPosition, 
            Mesh mesh,
            double wheelLength)
        {
            var center = Get(xPosition.CenterXPosition);
            if (center == null)
            {
                return null;
            }

            var left = Map(xPosition.LeftXPosition);
            if (left == null)
            {
                return null;
            }

            var right = Map(xPosition.RightXPosition);
            if (right == null)
            {
                return null;
            }

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
        /// Добирает координаты для проверок с учётом ширины тележек
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
            var wheelOffsetsMap = PassTypeFormulas.DistanceBetweenTrajectoryCenterAndAxles(loadModel.Axles);

            var trajectoryFilters = trajectoryFilterProvider.GetFilters(passageInterval, loadModel, roadRules);
            foreach (var filteredX in distinctXs.Where(x => trajectoryFilters.Any(filter => filter.Filter(x))))
            {
                result.Add(GetXPostition(filteredX, wheelOffsetsMap.Keys));
            }

            foreach (var edge in trajectoryFilters.Select(filter => (filter.EdgeCaseLeft, filter.EdgeCaseRight)))
            {
                result.Add(GetXPostition(edge.EdgeCaseLeft, wheelOffsetsMap.Keys));
                result.Add(GetXPostition(edge.EdgeCaseRight, wheelOffsetsMap.Keys));
            }

            return result.OrderBy(x => x.CenterXPosition).ToArray();
        }

        /// <summary>
        /// Расчёт напряжения от ТС
        /// </summary>
        /// <param name="trajectory">Траектория по которой двигается ТС</param>
        /// <param name="Y">Точка, в которой считаем напряжение</param>
        /// <param name="load">параметры нагрузки</param>
        /// <param name="invertAxles">ТС едет задом наперёд</param>
        /// <returns></returns>
        [MemberNotNull]
        public VehicleStrain GetStrainOnTrajectory(VehicleTrajectory trajectory, double Y, LoadModel load, bool invertAxles)
        {
            Func<Axle, double> axleFunc = invertAxles
            ? (axle) => { return Y - axle.AbsolutePosition; }
            : (axle) => { return Y + axle.AbsolutePosition; };

            IEnumerable<WheelStrain> wheelStrains = load.Axles.SelectMany(axle =>
                axle.WheelsDistance.SelectMany<double, WheelStrain>(distance =>
                {
                    var strain = profileYZService.GetStrain(trajectory.Left[distance], axleFunc(axle), axle.WheelWeight);
                    var leftWheel = new WheelStrain
                    {
                        Position = new Vector2D
                        {
                            X = trajectory.Left[distance].X,
                            Y = axleFunc(axle)
                        },
                        AxleRef = axle,
                        Strain = strain
                    };
                    strain = profileYZService.GetStrain(trajectory.Right[distance], axleFunc(axle), axle.WheelWeight);
                    var rightWheel = new WheelStrain
                    {
                        Position = new Vector2D
                        {
                            X = trajectory.Right[distance].X,
                            Y = axleFunc(axle)
                        },
                        AxleRef = axle,
                        Strain = strain
                    };
                    return [leftWheel, rightWheel];
                })
            );

            return new VehicleStrain
            {
                SumStrain = wheelStrains.Sum(x => x.Strain),
                WheelStrains = wheelStrains.ToArray()
            };
        }

        public VehicleTrajectory? GetVehicleTrajectory(Mesh mesh, LoadModel loadModel, double centerXPosition)
        {
            var wheelOffsetsMap = PassTypeFormulas.DistanceBetweenTrajectoryCenterAndAxles(loadModel.Axles);
            var xPosition = GetXPostition(centerXPosition, wheelOffsetsMap.Keys);
            return GetVehicleTrajectory(xPosition, mesh, centerXPosition);
        }

        private VehicleXPosition GetXPostition(double centerXPosition, IEnumerable<double> halfWheelOffsets)
        {
            var left = new Dictionary<double, double>();
            var right = new Dictionary<double, double>();
            foreach (var halfWheelOffset in halfWheelOffsets)
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
}
