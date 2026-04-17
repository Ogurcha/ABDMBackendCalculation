using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Graphics.Models;
using Abdm.Calculation.Maths.Extensions;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.GraphicsServices
{
    public class VehicleTrajectoryService(
        IEqualityComparer<double> equalityComparer,
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
                dataModel.Data.Load.Axles,
                dataModel.Data.Load.ActualDirection);

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
            var firstIndex = sorted.First().y - wheelLength;
            var lastIndex = sorted.Last().y + wheelLength;
            var firstVector = new Vector2D(firstIndex, 0);
            var lastVector = new Vector2D(lastIndex, 0);

            var sortedFullList = sorted.Select((item) => new Vector2D(item.y, item.z))
                .Prepend(firstVector)
                .Append(lastVector);

            var vectors = new SortedList<double, Vector2D>(
                sorted.Select((item) => new KeyValuePair<double, Vector2D>(item.y, (item.y, item.z)))
                .ToDictionary());

            var (extremums, maximums) = MathExtensions.FindAllExtremums(sortedFullList);

            return new ProfileYZ
            {
                X = X,
                Vectors = vectors,
                Extremums = extremums.ToArray(),
                MaximumIndexes = maximums.ToArray()
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
            Axle[] axles,
            bool[] direction)
        {
            return vehicleXPositions
                .Select(x => GetVehicleTrajectoryBase(x, mesh, axles, direction))
                .OfType<VehicleTrajectory>()
                .ToArray();
        }

        public VehicleTrajectory? GetVehicleTrajectoryBase(VehicleXPosition xPosition,
            Mesh mesh,
            Axle[] axles,
            bool[] directions)
        {
            var wheelLengthAvg = axles.Select(x => x.Wx).Average();

            ProfileYZ? Get(double x) => GetProfileYZ(mesh, x, wheelLengthAvg);

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

            //if (settings.UseSuperProfiles)
            //{
            //    var superProfiles = directions.Select(d =>
            //    new KeyValuePair<bool, ProfileYZ>(d,
            //    new ProfileYZ() { 
            //        Vectors = GetSuperProfileVectors(trajectory, axles, !d), 
            //        X = center.X }))
            //        .ToList();

            //    trajectory.SuperProfile = new Dictionary<bool, ProfileYZ>(superProfiles);
            //}

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
            return GetVehicleTrajectoryBase(xPosition, mesh, loadModel.Axles, loadModel.ActualDirection);
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
        /// Экспериментальный метод по поиску супер-профиля, который возникает, если сложить вместе все профили по каждоый оси ТС со сдвигами, соответствующими расстоянию на каждой оси. Получение суперпрофиля позволило бы при каждом просчете напряжения не считать напряжения на каждом колесе и складывать, а сразу считать напряжение от ТС в точке. Также это дает возможность не считать напряжение отдельно на каждом положительном интервале, а брать максимальное значение сразу. Но по прикидкам, поиск суперпрофиля - слишком длительный процесс. Выигрыш по времени если и есть (что не факт), то минимален. Но, возможно, стоит рассмотреть данный вариант, если будет возможность кэшировать суперпрофили (как-нибудь ночью в фоновом режиме).
        /// </summary>
        private SortedList<double, Vector2D> GetSuperProfileVectors(VehicleTrajectory vehicleTrajectory, 
            Axle[] axles, 
            bool invertAxles)
        {
            Func<Axle, double, double> axleFunc = invertAxles
            ? (axle, x) => { return x - axle.AbsolutePosition; }
            : (axle, x) => { return x + axle.AbsolutePosition; };

            var wheelsValues = axles.SelectMany(axle => axle.WheelsDistance.Select(wheelDistanceItem => (wheelDistanceItem, axle)));

            var distinctXs = vehicleTrajectory.Center.Vectors.Select(center => center.Value.X)
            .SelectMany(x => wheelsValues,
            (x, wheel) => axleFunc(wheel.axle, x)).Distinct(equalityComparer);

            return new SortedList<double, Vector2D>
                (distinctXs.Select(x =>
                    new Vector2D
                    {
                        X = x,
                        Y = wheelsValues.Sum(wheelValue =>
                            vehicleTrajectory.Left[wheelValue.wheelDistanceItem].GetZValueByY(axleFunc(wheelValue.axle, x))
                            * wheelValue.axle.WheelWeight
                            + vehicleTrajectory.Right[wheelValue.wheelDistanceItem].GetZValueByY(axleFunc(wheelValue.axle, x))
                            * wheelValue.axle.WheelWeight)
                    }).Select((item) => new KeyValuePair<double, Vector2D>(item.X, item))
                        .ToDictionary()
                );
        }
    }
}
