using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services.LowLevelCalculation
{
    public class ProfileYZServiceVolumetric(
        IMeshManager meshManager,
        ITrajectoryFilterProvider trajectoryFilterProvider
        ) : ProfileYZService(meshManager, trajectoryFilterProvider), IProfileYZServiceVolumetric
    {
        /// <summary>
        /// Возвращает расширенный профиль <see cref="ProfileYZ"/> для случаев, 
        /// когда необходимо считать объёмы поверхности влияния под полосой
        /// </summary>
        public ProfileYZExtended? GetProfileYZVolumetric(Mesh mesh,
            ProfileYZ profile,
            IEnumerable<Axle> axles,
            double coatLength,
            Dictionary<double, ProfileYZ> profileMap)
        {
            var volumetricProfiles = new Dictionary<Axle, ProfileYZBase[]>();
            var footprintLength = new Dictionary<Axle, double>();
            var footprintWidth = new Dictionary<Axle, double>();

            var edgeProfiles = new Dictionary<double, ProfileYZBase>();

            foreach (var axle in axles)
            {
                footprintWidth[axle] = axle.WheelWidth + coatLength * 2;
                footprintLength[axle] = axle.WheelLength + coatLength * 2;

                var edgeLeft = profile.X - footprintWidth[axle] / 2;
                var edgeRight = profile.X + footprintWidth[axle] / 2;

                if (!edgeProfiles.ContainsKey(edgeLeft))
                {
                    var sortedVectorsLeft = GetIntersectionVectorsSorted(mesh, edgeLeft);
                    if (sortedVectorsLeft == null)
                    {
                        return null;
                    }
                    edgeProfiles.Add(edgeLeft, new ProfileYZBase { SortedVectors = sortedVectorsLeft, X = edgeLeft });
                }

                if (!edgeProfiles.ContainsKey(edgeRight))
                {
                    var sortedVectorsRight = GetIntersectionVectorsSorted(mesh, edgeRight);
                    if (sortedVectorsRight == null)
                    {
                        return null;
                    }
                    edgeProfiles.Add(edgeRight, new ProfileYZBase { SortedVectors = sortedVectorsRight, X = edgeRight });
                }

                var list = new List<ProfileYZBase>(profileMap.Where(p => edgeLeft < p.Key && p.Key < edgeRight).Select(x => x.Value))
                {
                    edgeProfiles[edgeLeft],
                    edgeProfiles[edgeRight]
                };
                volumetricProfiles[axle] = list.OrderBy(p => p.X).ToArray();
            }

            return new ProfileYZExtended
            {
                X = profile.X,
                SortedVectors = profile.SortedVectors,
                Extremums = profile.Extremums,
                MaximumIndexes = profile.MaximumIndexes,
                PositivePieces = profile.PositivePieces,
                PositivePieceMap = profile.PositivePieceMap,
                FootprintLength = footprintLength,
                FootprintWidth = footprintWidth,
                VolumetricProfiles = volumetricProfiles,
            };
        }
    }
}
