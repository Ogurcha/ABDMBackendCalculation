using System.Data;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainSelector(
        IVehicleTrajectoryService vehicleTrajectoryService, 
        IStrainCalculator strainCalculator, 
        IEqualityComparer<double> equalityComparer,
        ITrajectoryFilterProvider trajectoryFilterProvider) : IStrainSelector
    {
        public IEnumerable<StrainResultUnpopulated> SelectBestStrainResult(
        Dictionary<RoadRule, StrainsInMaximums[]> strainsMap,
        IntervalModel intervalModel,
        VehicleRollingBigModel bigData)
        {
            var data = bigData.Data;
            var mesh = bigData.Mesh;
            var roadRules = bigData.RoadRules;
            foreach (var roadRule in roadRules)
            {
                if (!strainsMap[roadRule].Any())
                {
                    continue;
                }

                var actualVehicleCount = Math.Min(roadRule.MaxTrajectoriesCount, intervalModel.PassageIntervalRef.LaneCount);
                
                if (actualVehicleCount == 1)
                {
                    yield return
                        new StrainResultUnpopulated
                        {
                            RoadRuleRef = roadRule,
                            Strain = [strainsMap[roadRule].First()]
                        };
                }
                else
                {
                    var strainResult = GetStrainResult(strainsMap[roadRule], intervalModel, roadRule, data, mesh, actualVehicleCount);
                    if (strainResult == null) 
                    { 
                        continue; 
                    }
                    yield return strainResult!;
                }
            }
        }

        /// <summary>
        /// Выбирает подходящие траектории движения для случая, если двигаются более одного ТС. 
        /// Траектории выбираются исходя из максимального напряжения, но ТС не должны "налезать" друг на друга. 
        /// Но также проверяется оптимальная траектория рядом с уже установленным ТС 
        /// </summary>
        /// <param name="strains">Напряжения c координатой траектории</param>
        /// <param name="intervalModel">интервал моста, внутри которого происходит движение</param>
        /// <param name="roadRule">Правила движения по мосту</param>
        /// <param name="data">Параметры поверхности и нагрузки</param>
        /// <param name="mesh">Поверхность влияния</param>
        /// <param name="actualVehicleCount">Количество ТС</param>
        /// <returns></returns>
        private StrainResultUnpopulated? GetStrainResult(StrainsInMaximums[] strains,
            IntervalModel intervalModel,
            RoadRule roadRule,
            VehicleRollingSmallModel data,
            Mesh mesh,
            int actualVehicleCount)
        {
            var orderedByPosition = new LinkedList<StrainsInMaximums>();
            var orderedByStrain = new List<LinkedListNode<StrainsInMaximums>>(strains.OrderBy(x => x.X).Select(orderedByPosition.AddLast).OrderBy(x => x.Value.TotalStrain));

            var trajectoryFilter = trajectoryFilterProvider.GetFilter(intervalModel.PassageIntervalRef, data.Load, roadRule);
            List<StrainsInMaximums> vehicleStrains = new();

            while (vehicleStrains.Count >= actualVehicleCount && orderedByPosition.Count > 0 && orderedByStrain.Count > 0)
            {
                var node = orderedByStrain.Last();
                orderedByStrain.Remove(node);
                if (node.List == orderedByPosition)
                {
                    UseStrain(node);
                }
            }

            if (vehicleStrains.Count == 0)
            {
                return null;
            }
            else
            {
                var strainResult = new StrainResultUnpopulated
                {
                    RoadRuleRef = roadRule,
                    Strain = vehicleStrains.ToArray()
                };

                return strainResult;
            }

            void UseStrain(LinkedListNode<StrainsInMaximums> node)
            {
                vehicleStrains.Add(node.Value);
                var center = node.Value.X;
                var radius = Math.Max(roadRule.MinTrajectoryDistance, data.Load.Interval);
                var left = center - radius;
                var right = center + radius;

                RemoveNodesNearCenter(
                    orderedByPosition, 
                    node, 
                    center, 
                    radius,
                    out LinkedListNode<StrainsInMaximums>? edgeNode1, 
                    out LinkedListNode<StrainsInMaximums>? edgeNode2);

                TryAddTrajectory(left, edgeNode1 != null 
                    ? (StrainsInMaximums x) => orderedByPosition.AddAfter(edgeNode1, x)
                    : orderedByPosition.AddLast,
                    edgeNode1);
                TryAddTrajectory(left, edgeNode2 != null 
                    ? (StrainsInMaximums x) => orderedByPosition.AddBefore(edgeNode2, x)
                    : orderedByPosition.AddFirst,
                    edgeNode2);
            }

            void TryAddTrajectory(double traj, 
                Func<StrainsInMaximums, LinkedListNode<StrainsInMaximums>> insertFunc,
                LinkedListNode<StrainsInMaximums>? edgeNode)
            {
                if (!(edgeNode?.Value?.X == traj)
                    && trajectoryFilter.Filter(traj)
                    && !strains.Select(s => s.X).Contains(traj, equalityComparer)
                    && vehicleTrajectoryService.GetVehicleTrajectory(mesh, data.Load, traj) is VehicleTrajectory additionalTrajectory
                    && strainCalculator.TryGetStrainForEachPositivePiece(additionalTrajectory, data, out IEnumerable<VehicleStrain> vehicleStrains))
                {
                    var strains = vehicleStrains.OrderDescending().ToArray();
                    var trafficJamStrain = roadRule.DoTrafficJamLoadCalulation
                        ? strainCalculator.GetTrafficJamStrain(additionalTrajectory, data)
                        : null;
                    var additionalStrain = new StrainsInMaximums
                    {
                        VehicleTrajectoryRef = additionalTrajectory,
                        Strains = strains,
                        TrafficJamStrain = trafficJamStrain,
                        TotalStrain = strains.First().TotalStrain + trafficJamStrain?.TotalStrain ?? 0d
                    };
                    var additionalStrainNode = insertFunc(additionalStrain);
                    orderedByStrain.Add(additionalStrainNode);
                    orderedByStrain = orderedByStrain.OrderBy(x => x.Value.TotalStrain).ToList();
                }
            }
        }

        /// <summary>
        /// Оптимизированное исключение напряжений. При повторном вызове связный список уже не будет содержать исключённые напряжения
        /// </summary>
        private void RemoveNodesNearCenter(
            LinkedList<StrainsInMaximums> list,
            LinkedListNode<StrainsInMaximums> linkedListNode,
            double center,
            double radius,
            out LinkedListNode<StrainsInMaximums>? edgeNode1,
            out LinkedListNode<StrainsInMaximums>? edgeNode2)
        {
            double minPos = center - radius;
            double maxPos = center + radius;
            edgeNode1 = null;
            edgeNode2 = null;

            var node = linkedListNode.Previous;
            while (node != null)
            {
                var prev = node.Previous;
                if (node.Value.X > minPos && !equalityComparer.Equals(minPos, node.Value.X))
                {
                    list.Remove(node);
                }
                else
                {
                    edgeNode1 = node;
                    break;
                }
                node = prev;
            }

            node = linkedListNode.Next;
            while (node != null)
            {
                var next = node.Next;
                if (node.Value.X < maxPos && !equalityComparer.Equals(maxPos, node.Value.X))
                {
                    list.Remove(node);
                }
                else
                {
                    edgeNode2 = node;
                    break;
                }
                node = next;
            }

            if (linkedListNode.List == list)
            {
                list.Remove(linkedListNode);
            }
        }
    }
}
