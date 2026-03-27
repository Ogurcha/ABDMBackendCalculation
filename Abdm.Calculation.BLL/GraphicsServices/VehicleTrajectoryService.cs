using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Extensions;
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
            var firstVector = new KeyValuePair<double, Vector2D>(sorted.First().y, (sorted.First().y - wheelLength, 0)); 
            var lastVector = new KeyValuePair<double, Vector2D>(sorted.Last().y, (sorted.Last().y + wheelLength, 0)); 
            var vectors = new SortedList<double, Vector2D>(
                sorted.Select((item) => new KeyValuePair<double, Vector2D>(item.y, (item.y, item.z)))
                .Prepend(firstVector)
                .Append(lastVector)
                .ToDictionary());

            return new ProfileYZ
            {
                X = X,
                Vectors = vectors,
                Extremums = FindAllExtremums(vectors).ToArray() 
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
            ProfileYZ? Get(double x) => GetProfileYZ(mesh, x, wheelLength);

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

            return new VehicleTrajectory
            {
                Center = center,
                Left = left,
                Right = right
            };
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
        public VehicleStrain GetStrainOnTrajectory(VehicleTrajectory trajectory, double Y, LoadModel load, bool invertAxles)
        {
            Func<Axle, double> axleFunc = invertAxles
            ? (axle) => { return Y - axle.AbsolutePosition; }
            : (axle) => { return Y + axle.AbsolutePosition; };

            IEnumerable<WheelStrain> wheelStrains = load.Axles.SelectMany(axle =>
                axle.WheelsDistance.SelectMany<double, WheelStrain>(distance =>
                {
                    var strain = trajectory.Left[distance].GetStrain(axleFunc(axle), axle.WheelWeight);
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
                    strain = trajectory.Right[distance].GetStrain(axleFunc(axle), axle.WheelWeight);
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
                WheelStrains = wheelStrains.ToArray(),
                VehicleTrajectoryRef = trajectory,
                IsDirectionForward = !invertAxles
            };
        }

        public VehicleTrajectory? GetVehicleTrajectory(Mesh mesh, LoadModel loadModel, double centerXPosition)
        {
            var wheelOffsetsMap = PassTypeFormulas.DistanceBetweenTrajectoryCenterAndAxles(loadModel.Axles);
            var xPosition = GetXPostition(centerXPosition, wheelOffsetsMap.Keys);
            return GetVehicleTrajectory(xPosition, mesh, wheelOffsetsMap.Keys.Average());
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

        /// <summary>
        /// Находит все строгие локальные экстремумы функции, заданной отсортированным списком точек.
        /// Сложность: O(n), один проход по данным.
        /// </summary>
        /// <param name="sortedPoints">
        /// SortedList<double, Vector2d>, где Key — X, Value.Y — f(X).
        /// Список должен быть отсортирован по возрастанию Key.
        /// </param>
        /// <returns>Список всех локальных максимумов и минимумов.</returns>
        public static List<ProfileExtremum> FindAllExtremums(SortedList<double, Vector2D> sortedPoints)
        {
            var result = new List<ProfileExtremum>();

            if (sortedPoints.Count < 3)
            {
                return result;
            }
                
            var keys = sortedPoints.Keys;
            var values = sortedPoints.Values;

            for (int i = 1; i < sortedPoints.Count - 1; i++)
            {
                double yPrev = values[i - 1].Y;
                double yCurr = values[i].Y;
                double yNext = values[i + 1].Y;

                bool isMax = yPrev < yCurr && yCurr > yNext;
                bool isMin = yPrev > yCurr && yCurr < yNext;

                if (isMax || isMin)
                {
                    result.Add(new ProfileExtremum
                    {
                        Position = keys[i],
                        isMaximum = isMax
                    });
                }
            }

            return result;
        }
    }
}
