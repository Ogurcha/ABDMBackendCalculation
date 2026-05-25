using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Graphics.Models;
using Abdm.Calculation.Maths.Extensions;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.GraphicsServices
{
    public class VehicleTrajectoryService(
        IMeshManager meshManager,
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
                dataModel.Mesh);

            return result;
        }

        private const double smallValue = 0.5e-10d;

        /// <summary>
        /// Возвращает пересечение с поверхностью
        /// Вернёт null - если пересечения нет
        /// По краям сразу зануляем профиль, чтобы ТС вышедшее краем за пределы не влияло на результат
        /// </summary>
        /// <param name="wheelLength">длина колеса нужна для зануления профиля</param>
        public ProfileYZ? GetProfileYZ(Mesh mesh,
            double X)
        {
            var profile = meshManager.GetIntersectionVectors(mesh, X);

            if (profile?.Any() != true)
            {
                return null;
            }

            var sorted = profile.OrderBy(v => v.y);
            var firstIndex = sorted.First().y - smallValue;
            var lastIndex = sorted.Last().y + smallValue;
            var firstVector = new Vector2D(firstIndex, 0);
            var lastVector = new Vector2D(lastIndex, 0);

            var sortedFullList = sorted.Select((item) => new Vector2D(item.y, item.z))
                .Prepend(firstVector)
                .Append(lastVector);

            var vectors = new SortedList<double, Vector2D>(
                sortedFullList.Select((item) => new KeyValuePair<double, Vector2D>(item.X, (item.X, item.Y)))
                .ToDictionary());

            var (extremums, maximums, positivePieces, positivePiecesMap) = MathExtensions.FindExtremumsAndPositives(sortedFullList);

            if (maximums.Count == 0)
            {
                return null;
            }

            return new ProfileYZ
            {
                X = X,
                Vectors = vectors,
                Extremums = extremums.ToArray(),
                MaximumIndexes = maximums.ToArray(),
                PositivePieces = positivePieces.ToArray(),
                PositivePieceMap = positivePiecesMap
            };
        }

        /// <summary>
        /// Получение траекторий движения ТС
        /// По краям сразу зануляем профили, чтобы ТС вышедшее частично за пределы проекций не влияло на результат
        /// </summary>
        /// <param name="wheelLength">длина колеса нужна для зануления профиля по краям</param>
        /// <returns></returns>
        public VehicleTrajectory[] GetVehicleTrajectories([DisallowNull] VehicleXPosition[] vehicleXPositions,
            Mesh mesh)
        {
            return vehicleXPositions
                .Select(x => GetVehicleTrajectoryBase(x, mesh))
                .OfType<VehicleTrajectory>()
                .ToArray();
        }

        public VehicleTrajectory? GetVehicleTrajectoryBase(VehicleXPosition xPosition,
            Mesh mesh)
        {
            ProfileYZ? Get(double x) => GetProfileYZ(mesh, x);

            var center = Get(xPosition.CenterXPosition);
            if (center == null)
            {
                return null;
            }

            Dictionary<double, ProfileYZ>? Map(Dictionary<double, double> positions)
            {
                var resolved = positions
                    .Select(kv => (kv.Key, Value: Get(kv.Value)))
                    .ToList();

                if (resolved.Any(r => r.Value == null))
                {
                    return null;
                }

                return resolved.ToDictionary(r => r.Key, r => r.Value!);
            }

            var leftDict = Map(xPosition.LeftXPosition);
            if (leftDict == null)
            {
                return null;
            }

            var rightDict = Map(xPosition.RightXPosition);
            if (rightDict == null)
            {
                return null;
            }

            var left = new SortedList<double, ProfileYZ>(leftDict);
            var right = new SortedList<double, ProfileYZ>(rightDict);

            var trajectory = new VehicleTrajectory
            {
                Center = center,
                Left = left,
                Right = right
            };

            return trajectory;
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

            var trajectoryFilters = trajectoryFilterProvider.GetFilters(passageInterval, loadModel, roadRules);
            foreach (var filteredX in distinctXs.Where(x => trajectoryFilters.Any(filter => filter.Filter(x))))
            {
                result.Add(GetXPosition(filteredX, loadModel.WheelOffsetsMap!.Keys));
            }

            foreach (var edge in trajectoryFilters.Select(filter => (filter.EdgeCaseLeft, filter.EdgeCaseRight)))
            {
                result.Add(GetXPosition(edge.EdgeCaseLeft, loadModel.WheelOffsetsMap!.Keys));
                result.Add(GetXPosition(edge.EdgeCaseRight, loadModel.WheelOffsetsMap!.Keys));
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
        public VehicleStrain GetStrainOnTrajectory(VehicleTrajectory trajectory, 
            double Y, 
            LoadModel load, 
            bool invertAxles)
        {
            Func<Axle, double> axleFunc = invertAxles
            ? (axle) => { return Y - axle.AbsolutePosition; }
            : (axle) => { return Y + axle.AbsolutePosition; };

            var positivePiecesMap = new Dictionary<ProfileYZ, HashSet<Interval>>();
            foreach (var profile in trajectory.Left)
            {
                positivePiecesMap.Add(profile.Value, new HashSet<Interval>());
            }
            foreach (var profile in trajectory.Right)
            {
                positivePiecesMap.Add(profile.Value, new HashSet<Interval>());
            }

            IEnumerable<WheelStrain> wheelStrains = load.Axles.SelectMany(axle =>
                axle.WheelsDistance.SelectMany<double, WheelStrain>(distance =>
                {
                    var strain = trajectory.Left[distance].GetZValueByY(axleFunc(axle), out (Interval? i1, Interval? i2) positivePiecesLeft);
                    var leftWheel = new WheelStrain
                    {
                        Position = new Vector2D
                        {
                            X = trajectory.Left[distance].X,
                            Y = axleFunc(axle)
                        },
                        AxleRef = axle,
                        Strain = strain,
                    };
                    if (positivePiecesLeft.i1 != null)
                    {
                        positivePiecesMap[trajectory.Left[distance]].Add(positivePiecesLeft.i1);
                    }
                    if (positivePiecesLeft.i2 != null)
                    {
                        positivePiecesMap[trajectory.Left[distance]].Add(positivePiecesLeft.i2);
                    }

                    strain = trajectory.Right[distance].GetZValueByY(axleFunc(axle), out (Interval? i1, Interval? i2) positivePiecesRight);
                    var rightWheel = new WheelStrain
                    {
                        Position = new Vector2D
                        {
                            X = trajectory.Right[distance].X,
                            Y = axleFunc(axle)
                        },
                        AxleRef = axle,
                        Strain = strain,
                    };
                    if (positivePiecesRight.i1 != null)
                    {
                        positivePiecesMap[trajectory.Right[distance]].Add(positivePiecesRight.i1);
                    }
                    if (positivePiecesRight.i2 != null)
                    {
                        positivePiecesMap[trajectory.Right[distance]].Add(positivePiecesRight.i2);
                    }

                    return [leftWheel, rightWheel];
                })
            );


            return new VehicleStrain
            {
                SumStrain = wheelStrains.Sum(x => x.Strain),
                WheelStrains = wheelStrains.ToArray(),
                IsDirectionForward = !invertAxles,
                PositivePiecesMap = positivePiecesMap,
                Position = Y
            };
        }

        public VehicleTrajectory? GetVehicleTrajectory(Mesh mesh, LoadModel loadModel, double centerXPosition)
        {
            var xPosition = GetXPosition(centerXPosition, loadModel.WheelOffsetsMap!.Keys);
            return GetVehicleTrajectoryBase(xPosition, mesh);
        }

        private VehicleXPosition GetXPosition(double centerXPosition, IEnumerable<double> halfWheelOffsets)
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
