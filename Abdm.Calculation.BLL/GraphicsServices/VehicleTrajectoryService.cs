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
                dataModel,
                interval);

            Func<VehicleXPosition, VehicleTrajectory?> trajFunc;
            if (dataModel.Data.Surface.StrainCalculationGroupType == Enums.StrainCalculationGroupTypeEnum.Slab
                || dataModel.RoadRules.Any(r => r.DoTrafficJamLoadCalculation))
            {
                trajFunc = x => GetVehicleTrajectoryBaseWithExtendedProfiles(x, 
                    dataModel.Mesh, 
                    dataModel.Data.Load.Axles, 
                    dataModel.Data.Surface.RoadCoatSize);
            }
            else
            {
                trajFunc = x => GetVehicleTrajectoryBase(x, dataModel.Mesh);
            }

            result.Trajectories = distinctXs
                .Select(x => trajFunc(x))
                .OfType<VehicleTrajectory>()
                .ToArray();

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
            var sortedFullList = GetIntersectionVectorsSorted(mesh, X);

            if (sortedFullList == null)
            {
                return null;
            }

            var (extremums, maximums, positivePieces, positivePiecesMap) = MathExtensions.FindExtremumsAndPositives(sortedFullList);

            if (maximums.Count == 0)
            {
                return null;
            }

            return new ProfileYZ
            {
                X = X,
                SortedVectors = sortedFullList.ToArray(),
                Extremums = extremums.ToArray(),
                MaximumIndexes = maximums.ToArray(),
                PositivePieces = positivePieces.ToArray(),
                PositivePieceMap = positivePiecesMap
            };
        }

        /// <summary>
        /// Возвращает расширенный профиль <see cref="ProfileYZ"/> для случаев, 
        /// когда необходимо считать объёмы поверхности влияния под полосой
        /// </summary>
        public ProfileYZExtended? GetProfileYZExtended(Mesh mesh,
            double X,
            Axle[] axles,
            double coatLength)
        {
            var profile = GetProfileYZ(mesh, X);
            if (profile == null)
            {
                return null;
            }

            //TODO#2: Доделать ProfileYZExtended для случая, если в нагрузке много и РАЗНЫХ Axle
            var axle = axles.First();
            var distMin = axle.WheelWidth / 2;

            var sortedVectors1 = GetIntersectionVectorsSorted(mesh, X - distMin);
            if (sortedVectors1 == null)
            {
                return null;
            }
            var sortedVectors2 = GetIntersectionVectorsSorted(mesh, X + distMin);
            if (sortedVectors2 == null)
            {
                return null;
            }

            return new ProfileYZExtended
            {
                X = profile.X,
                SortedVectors = profile.SortedVectors,
                Extremums = profile.Extremums,
                MaximumIndexes = profile.MaximumIndexes,
                PositivePieces = profile.PositivePieces,
                PositivePieceMap = profile.PositivePieceMap,
                FootprintLength = axle.WheelLength + 2 * coatLength,
                FootprintWidth = axle.WheelWidth + 2 * coatLength,
                SortedVectorsLeft = sortedVectors1.ToArray(),
                SortedVectorsRight = sortedVectors2.ToArray(),
            };
        }

        public IEnumerable<Vector2D>? GetIntersectionVectorsSorted(Mesh mesh, double X)
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

            return sorted.Select((item) => new Vector2D(item.y, item.z))
                .Prepend(firstVector)
                .Append(lastVector);
        }

        /// <summary>
        /// Получение траекторий движения ТС
        /// По краям сразу зануляем профили, чтобы ТС вышедшее частично за пределы проекций не влияло на результат
        /// </summary>
        /// <param name="wheelLength">длина колеса нужна для зануления профиля по краям</param>
        /// <returns></returns>
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

        private VehicleTrajectory? GetVehicleTrajectoryBaseWithExtendedProfiles(VehicleXPosition xPosition, Mesh mesh, Axle[] axles, double roadCoatSize)
        {
            ProfileYZ? Get(double x) => GetProfileYZ(mesh, x);
            ProfileYZ? GetExt(double x) => GetProfileYZExtended(mesh, x, axles, roadCoatSize);

            var center = Get(xPosition.CenterXPosition);
            if (center == null)
            {
                return null;
            }

            Dictionary<double, ProfileYZ>? Map(Dictionary<double, double> positions)
            {
                var resolved = positions
                    .Select(kv => (kv.Key, Value: GetExt(kv.Value)))
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
            VehicleRollingBigModel dataModel,
            PassageInterval passageInterval)
        {
            var result = new List<VehicleXPosition>();
            var distinctXs = dataModel.Mesh.Data.DistinctXs;
            var loadModel = dataModel.Data.Load;
            var roadRules = dataModel.RoadRules;
            var surface = dataModel.Data.Surface;

            var trajectoryFilters = trajectoryFilterProvider.GetFilters(passageInterval, loadModel, roadRules);

            if (surface.StrainCalculationGroupType == Enums.StrainCalculationGroupTypeEnum.Slab)
            {
                foreach (var filteredX in distinctXs.Where(x => trajectoryFilters.Any(filter => filter.Filter(x))))
                {
                    foreach (var wheelOffset in loadModel.WheelOffsetsMapCentered!)
                    {
                        result.Add(GetXPosition(filteredX - wheelOffset.Key, loadModel.WheelOffsetsMap!.Keys));
                        result.Add(GetXPosition(filteredX + wheelOffset.Key, loadModel.WheelOffsetsMap!.Keys));
                    }
                }
            }
            else
            {
                foreach (var filteredX in distinctXs.Where(x => trajectoryFilters.Any(filter => filter.Filter(x))))
                {
                    result.Add(GetXPosition(filteredX, loadModel.WheelOffsetsMap!.Keys));
                }
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
            bool invertAxles,
            bool doSlabCalculation)
        {
            Func<Axle, double> axleFunc = invertAxles
            ? (axle) => { return Y - axle.Position; }
            : (axle) => { return Y + axle.Position; };

            var positivePiecesMap = new Dictionary<ProfileYZ, HashSet<Interval>>();
            foreach (var profile in trajectory.Left)
            {
                positivePiecesMap.Add(profile.Value, new HashSet<Interval>());
            }
            foreach (var profile in trajectory.Right)
            {
                positivePiecesMap.Add(profile.Value, new HashSet<Interval>());
            }

            IEnumerable<WheelStrain> wheelStrains;
            if (doSlabCalculation)
            {
                wheelStrains = load.Axles.SelectMany(axle =>
                axle.WheelsDistance.SelectMany<double, WheelStrain>(distance =>
                {
                    var leftWheel = GetWheelStrainSlab(trajectory.Left[distance], positivePiecesMap, axle, axleFunc);
                    var rightWheel = GetWheelStrainSlab(trajectory.Right[distance], positivePiecesMap, axle, axleFunc);
                    return [leftWheel, rightWheel];
                }));
            }
            else
            {
                wheelStrains = load.Axles.SelectMany(axle =>
                axle.WheelsDistance.SelectMany<double, WheelStrain>(distance =>
                {
                    var leftWheel = GetWheelStrain(trajectory.Left[distance], positivePiecesMap, axle, axleFunc);
                    var rightWheel = GetWheelStrain(trajectory.Right[distance], positivePiecesMap, axle, axleFunc);
                    return [leftWheel, rightWheel];
                }));
            }

            return new VehicleStrain
            {
                SumStrain = wheelStrains.Sum(x => x.Strain),
                WheelStrains = wheelStrains.ToArray(),
                IsDirectionForward = !invertAxles,
                PositivePiecesMap = positivePiecesMap,
                Position = Y
            };

            WheelStrain GetWheelStrain(ProfileYZ profile, Dictionary<ProfileYZ, HashSet<Interval>> positivePiecesMap, Axle axle, Func<Axle, double> axleFunc)
            {
                var zValue = profile.GetZValueByY(axleFunc(axle), out (Interval? i1, Interval? i2) positivePieces);
                var strain = zValue * axle.wheelWeight;
                var wheel = new WheelStrain
                {
                    Position = new Vector2D
                    {
                        X = profile.X,
                        Y = axleFunc(axle)
                    },
                    AxleRef = axle,
                    Strain = strain,
                    ZValue = zValue,
                };
                if (positivePieces.i1 != null)
                {
                    positivePiecesMap[profile].Add(positivePieces.i1);
                }
                if (positivePieces.i2 != null)
                {
                    positivePiecesMap[profile].Add(positivePieces.i2);
                }

                return wheel;
            }

            WheelStrain GetWheelStrainSlab(ProfileYZ profilebase, Dictionary<ProfileYZ, HashSet<Interval>> positivePiecesMap, Axle axle, Func<Axle, double> axleFunc)
            {
                if (profilebase is not ProfileYZExtended profile)
                {
                    return GetWheelStrain(profilebase, positivePiecesMap, axle, axleFunc);
                }
                var zValue = profile.GetZValueByYSlabVersion(axleFunc(axle), out (Interval? i1, Interval? i2) positivePieces);
                var strain = zValue * axle.wheelWeight;
                var wheel = new WheelStrain
                {
                    Position = new Vector2D
                    {
                        X = profile.X,
                        Y = axleFunc(axle)
                    },
                    AxleRef = axle,
                    Strain = strain,
                    ZValue = zValue,
                    FootprintLength = profile.FootprintLength,
                    FootprintWidth = profile.FootprintWidth,
                };
                if (positivePieces.i1 != null)
                {
                    positivePiecesMap[profile].Add(positivePieces.i1);
                }
                if (positivePieces.i2 != null)
                {
                    positivePiecesMap[profile].Add(positivePieces.i2);
                }

                return wheel;
            }
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
