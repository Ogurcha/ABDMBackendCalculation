using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainResultPopulator(IVehiclePositioner vehiclePositioner, IEqualityComparer<double> equalityComparer) : IStrainResultPopulator
    {
        public List<StrainResult> PopulateStrainResults(IList<StrainResultUnpopulated> list, VehicleRollingSmallModel data)
        {
            var strainResults = new List<StrainResult>();
            var strainResultsMap = new Dictionary<int, Dictionary<double, VehicleColumnStrain>>();
            foreach (var unpopulated in list)
            {
                var maxVehicles = unpopulated.RoadRuleRef.MaxVehicleInTrajectory;
                if (!strainResultsMap.ContainsKey(maxVehicles))
                {
                    strainResultsMap.Add(maxVehicles, new Dictionary<double, VehicleColumnStrain>(equalityComparer));
                }
                strainResults.Add(PopulateStrainResult(unpopulated, data, maxVehicles, strainResultsMap[maxVehicles]));
            }
            return strainResults;
        }

        /// <summary>
        /// Получив напряжения для локальных максимумов, а также дополнительные варианты напряжений, полученные в результате смещения этих максимумов на расстояния, кратные расстоянию между ТС в автоколонне, выбираем напряжения по убыванию, но таким образом, чтобы расстояние между позициями выбранных напряжений было не меньше расстояния между ТС в автоколонне, а количество напряжений было не больше максимального количества ТС в автоколонне <paramref name="data.RoadRuleRef.MaxVehicleInTrajectory"/>
        /// </summary>
        private StrainResult PopulateStrainResult(
            StrainResultUnpopulated unpopulated, 
            VehicleRollingSmallModel data, 
            int maxVehicle, 
            Dictionary<double, VehicleColumnStrain> resultsMap)
        {
            if (maxVehicle == 1)
            {
                return new StrainResult
                {
                    RoadRuleRef = unpopulated.RoadRuleRef,
                    Strain = unpopulated.Strain.Select(GetVehicleColumnStrain).ToArray(),
                    IntervalModelRef = unpopulated.IntervalModelRef
                };
            }

            return new StrainResult
            {
                RoadRuleRef = unpopulated.RoadRuleRef,
                Strain = unpopulated.Strain.Select(x => PopulateIndividualColumnFromMap(x, data, maxVehicle, resultsMap)).ToArray(),
                IntervalModelRef = unpopulated.IntervalModelRef
            };

            VehicleColumnStrain PopulateIndividualColumnFromMap(StrainsInMaximums traj,
                VehicleRollingSmallModel data,
                int maxVehicleInTrajectory,
                Dictionary<double, VehicleColumnStrain> resultsMap)
            {
                if (!resultsMap.ContainsKey(traj.X))
                {
                    resultsMap.Add(traj.X, PopulateIndividualColumn(traj, data, maxVehicleInTrajectory));
                }
                return resultsMap[traj.X];
            }
        }

        /// <summary>
        /// Получив напряжения для локальных максимумов <paramref name="traj"/>, а также дополнительные варианты напряжений, полученные в результате смещения этих максимумов на расстояния, кратные <paramref name="effectiveLoadDistance"/>, выбираем напряжения по убыванию, но таким образом, чтобы расстояние между позициями выбранных напряжений было не меньше <paramref name="effectiveLoadDistance"/>, а количество напряжений было не больше максимального количества ТС в автоколонне <paramref name="maxVehiclesInColumn"/>
        /// </summary>
        private VehicleColumnStrain PopulateIndividualColumn(StrainsInMaximums traj, 
            VehicleRollingSmallModel data, 
            int maxVehicleInTrajectory)
        {
            double effectiveLoadDistance = data.Load.Length + Math.Max(NormConstants.DefaultVehicleDistance, data.Load.Distance);

            List<VehicleStrain> vehicleStrainList = GetManyVariants(traj, data, effectiveLoadDistance);

            List<VehicleStrain> resultStrains = ChooseVehicleStrains(
                vehicleStrainList,
                maxVehicleInTrajectory,
                effectiveLoadDistance);

            return new VehicleColumnStrain
            {
                VehicleTrajectoryRef = traj.VehicleTrajectoryRef,
                TrafficJamStrain = traj.TrafficJamStrain,
                VehicleStrains = resultStrains.ToArray(),
                TotalStrain = resultStrains.Sum(x => x.TotalStrain) + (traj.TrafficJamStrain?.TotalStrain ?? 0d)
            };
        }

        /// <summary>
        /// Имея локальные максимумы напряжений <paramref name="traj"/>, получаем дополнительные варианты напряжений, в точках, удалённых от них на расстояние, кратное <paramref name="effectiveLoadDistance"/>. Это позволяет учесть эффект от нескольких машин, следующих друг за другом на расстоянии.
        /// </summary>
        private List<VehicleStrain> GetManyVariants(StrainsInMaximums traj, VehicleRollingSmallModel data, double effectiveLoadDistance)
        {
            var vehicleStrainList = new List<VehicleStrain>();
            foreach (var vehicleStrain in traj.Strains)
            {
                vehicleStrainList.Add(vehicleStrain);
                var distanceFromExtremum = effectiveLoadDistance;
                while (true)
                {
                    bool isValidMax = vehicleStrain.Position + distanceFromExtremum
                        <= data.Surface.MaxY + data.Load.Length;

                    bool isValidMin = vehicleStrain.Position - distanceFromExtremum
                        >= data.Surface.MinY - data.Load.Length;

                    if (!isValidMax && !isValidMin)
                    {
                        break;
                    }
                        
                    if (isValidMax && TryCloneVehicleStrain(traj, data, vehicleStrain, distanceFromExtremum, out VehicleStrain? clonedMax))
                    {
                        vehicleStrainList.Add(clonedMax!);
                    }
                        
                    if (isValidMin && TryCloneVehicleStrain(traj, data, vehicleStrain, -distanceFromExtremum, out VehicleStrain? clonedMin))
                    {
                        vehicleStrainList.Add(clonedMin!);
                    }
                        
                    distanceFromExtremum += effectiveLoadDistance;
                }
            }

            return vehicleStrainList;
        }

        private bool TryCloneVehicleStrain(StrainsInMaximums traj, VehicleRollingSmallModel data, VehicleStrain vehicleStrain, double distanceFromExtremum, out VehicleStrain? cloned)
        {
            cloned = CloneVehicleStrain(traj, data, vehicleStrain, distanceFromExtremum);
            if (cloned != null)
            {
                return true;
            }
            return false;
        }

        private VehicleStrain? CloneVehicleStrain(StrainsInMaximums traj, VehicleRollingSmallModel data, VehicleStrain vehicleStrain, double distanceFromExtremum)
        {
            var strain = vehiclePositioner.GetStrainFromVehicleInPosition(traj.VehicleTrajectoryRef, vehicleStrain.Position + distanceFromExtremum, data);
            if (strain != null && strain.SumStrain > 0d)
            {
                strain.Coefficient = vehicleStrain.Coefficient;
                strain.LambdaSmall = vehicleStrain.LambdaSmall;
                strain.TotalStrain = strain.SumStrain * strain.Coefficient;
                return strain;
            }
            return null;
        }

        /// <summary>
        /// Выбираем напряжения по убыванию, но таким образом, чтобы расстояние между позициями выбранных напряжений было не меньше <paramref name="effectiveLoadDistance"/>, а количество напряжений было не больше максимального количества ТС в колонне <paramref name="maxVehiclesInColumn"/>
        /// </summary>
        private List<VehicleStrain> ChooseVehicleStrains(
            List<VehicleStrain> allVehicleStrains, 
            int maxVehiclesInColumn, 
            double effectiveLoadDistance)
        {
            var orderedByPosition = new LinkedList<VehicleStrain>();
            var orderedByStrain = new List<LinkedListNode<VehicleStrain>>(allVehicleStrains.OrderBy(x => x.Position).Select(orderedByPosition.AddLast).OrderByDescending(x => x.Value.TotalStrain));

            var resultStrains = new List<VehicleStrain>();

            foreach (var node in orderedByStrain)
            {
                if (orderedByPosition.Count == 0 || resultStrains.Count >= maxVehiclesInColumn)
                {
                    break;
                }
                if (node.List == orderedByPosition)
                {
                    resultStrains.Add(node.Value);
                    RemoveNodesNearCenter(
                        orderedByPosition,
                        node,
                        node.Value.Position,
                        effectiveLoadDistance);
                }
            }

            return resultStrains;
        }

        /// <summary>
        /// Оптимизированное исключение напряжений. При повторном вызове связный список уже не будет содержать исключённые напряжения
        /// </summary>
        private void RemoveNodesNearCenter(
            LinkedList<VehicleStrain> list,
            LinkedListNode<VehicleStrain> linkedListNode,
            double center,
            double radius)
        {
            double minPos = center - radius;
            double maxPos = center + radius;

            var node = linkedListNode.Previous;
            while (node != null)
            {
                var prev = node.Previous;
                if (node.Value.Position > minPos && !equalityComparer.Equals(minPos, node.Value.Position))
                {
                    list.Remove(node);
                }
                else
                {
                    break;
                }
                node = prev;
            }

            node = linkedListNode.Next;
            while (node != null)
            {
                var next = node.Next;
                if (node.Value.Position < maxPos && !equalityComparer.Equals(maxPos, node.Value.Position))
                {
                    list.Remove(node);
                }
                else
                {
                    break;
                }
                node = next;
            }

            if (linkedListNode.List == list)
            {
                list.Remove(linkedListNode);
            }
        }

        private VehicleColumnStrain GetVehicleColumnStrain(StrainsInMaximums traj)
        {
            return new VehicleColumnStrain
            {
                VehicleTrajectoryRef = traj.VehicleTrajectoryRef,
                TrafficJamStrain = traj.TrafficJamStrain,
                VehicleStrains = [traj.Strains.First()],
                TotalStrain = traj.TotalStrain
            };
        }
    }
}
