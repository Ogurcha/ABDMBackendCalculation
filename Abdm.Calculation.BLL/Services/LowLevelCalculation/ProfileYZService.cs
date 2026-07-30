using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Graphics.Models;
using Abdm.Calculation.Maths.Extensions;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Services.LowLevelCalculation
{
    public class ProfileYZService(
        IMeshManager meshManager,
        ITrajectoryFilterProvider trajectoryFilterProvider
        ) : IProfileYZService
    {
        protected ITrajectoryFilterProvider TrajectoryFilterProvider => trajectoryFilterProvider;

        public virtual VehicleXPosition[] CalculateRequiredTrajectoryPositions(VehicleRollingBigModel dataModel, PassageInterval passageInterval, bool doTrajectoriesUnderWheels)
        {
            var result = new List<VehicleXPosition>();
            var distinctXs = dataModel.Mesh.Data.DistinctXs;
            var loadModel = dataModel.Data.Load;
            var roadRules = dataModel.RoadRules;
            var surface = dataModel.Data.Surface;

            var trajectoryFilters = trajectoryFilterProvider.GetFilters(passageInterval, loadModel, roadRules);
            var actualVehicleCount = Math.Min(dataModel.RoadRules.Max(x => x.MaxTrajectoriesInInterval), passageInterval.LaneCount);
            var radiuses = dataModel.RoadRules.Select(x => x.MinTrajectoryDistance).Distinct().ToArray();

            if (doTrajectoriesUnderWheels)
            {
                foreach (var X in distinctXs)
                {
                    foreach (var wheelOffset in loadModel.WheelOffsetsMap!.Keys)
                    {
                        AddPositions(X - wheelOffset);
                        AddPositions(X + wheelOffset);
                    }
                }
            }
            else
            {
                foreach (var X in distinctXs)
                {
                    AddPositions(X);
                }
            }

            foreach (var edge in trajectoryFilters.Select(filter => (filter.EdgeCaseLeft, filter.EdgeCaseRight)))
            {
                AddPositions(edge.EdgeCaseLeft);
                AddPositions(edge.EdgeCaseRight);
            }

            return result.DistinctBy(x => x.CenterXPosition).OrderBy(x => x.CenterXPosition).ToArray();

            void AddPositions(double x)
            {
                if (AddPosition(x))
                {
                    for (var i = 1; i < actualVehicleCount; i++)
                    {
                        foreach (var delta in radiuses.Select(r => i * r))
                        {
                            AddPosition(x + delta);
                            AddPosition(x - delta);
                        }
                    }
                }
            }

            bool AddPosition(double x)
            {
                if (trajectoryFilters.Any(filter => filter.Filter(x)))
                {
                    result.Add(GetXPosition(x, loadModel.WheelOffsetsMap!.Keys));
                    return true;
                }
                return false;
            }
        }

        public virtual Dictionary<double, ProfileYZ> CreateProfileMap(VehicleXPosition[] xPositions, VehicleRollingBigModel dataModel)
        {
            var result = new Dictionary<double, ProfileYZ>();

            foreach (var x in xPositions.SelectMany(x => x.LeftXPosition.Values.Union([x.CenterXPosition]).Union(x.RightXPosition.Values)))
            {
                var profile = GetProfileYZ(dataModel.Mesh, x);
                if (profile != null)
                {
                    result[x] = profile;
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает пересечение с поверхностью
        /// Вернёт null - если пересечения нет
        /// По краям сразу зануляем профиль, чтобы ТС вышедшее краем за пределы не влияло на результат
        /// </summary>
        /// <param name="wheelLength">длина колеса нужна для зануления профиля</param>
        protected virtual ProfileYZ? GetProfileYZ(Mesh mesh,
            double X)
        {
            var sortedFullList = GetIntersectionVectorsSorted(mesh, X);

            if (sortedFullList == null || sortedFullList.Length == 0)
            {
                return null;
            }

            var (extremums, maximums, positivePieces, positivePiecesMap) = MathExtensions.FindExtremumsAndPositives(sortedFullList);

            return new ProfileYZ
            {
                X = X,
                SortedVectors = sortedFullList,
                Extremums = extremums.ToArray(),
                MaximumIndexes = maximums.ToArray(),
                PositivePieces = positivePieces.ToArray(),
                PositivePieceMap = positivePiecesMap,
            };
        }

        protected virtual VehicleXPosition GetXPosition(double centerXPosition, IEnumerable<double> halfWheelOffsets)
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

        protected const double smallValue = 0.5e-10d;
        protected virtual Vector2D[]? GetIntersectionVectorsSorted(Mesh mesh, double X)
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
                .Append(lastVector)
                .ToArray();
        }
    }









}
