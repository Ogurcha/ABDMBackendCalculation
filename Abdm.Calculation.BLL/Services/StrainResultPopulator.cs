using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainResultPopulator(IVehiclePositioner vehiclePositioner) : IStrainResultPopulator
    {
        /// <summary>
        /// Получив напряжения для локальных максимумов <paramref name="unpopulated"/>, а также дополнительные варианты напряжений, полученные в результате смещения этих максимумов на расстояния, кратные расстоянию между ТС в автоколонне, выбираем напряжения по убыванию, но таким образом, чтобы расстояние между позициями выбранных напряжений было не меньше расстояния между ТС в автоколонне, а количество напряжений было не больше максимального количества ТС в автоколонне <paramref name="data.RoadRuleRef.MaxVehicleInTrajectory"/>
        /// </summary>
        public StrainResult PopulateStrainResult(StrainResultUnpopulated unpopulated, VehicleRollingSmallModel data)
        {
            var maxVehicle = unpopulated.RoadRuleRef.MaxVehicleInTrajectory;
            return new StrainResult
            {
                RoadRuleRef = unpopulated.RoadRuleRef,
                Strain = new VehicleColumnStrainList(unpopulated.Strain.Select(x => PopulateIndividualColumn(x, data, maxVehicle))),
                StrainOneAuto = PopulateIndividualColumn(unpopulated.StrainOneAuto, data, maxVehicle)
            };
        }

        /// <summary>
        /// Получив напряжения для локальных максимумов <paramref name="traj"/>, а также дополнительные варианты напряжений, полученные в результате смещения этих максимумов на расстояния, кратные <paramref name="effectiveLoadDistance"/>, выбираем напряжения по убыванию, но таким образом, чтобы расстояние между позициями выбранных напряжений было не меньше <paramref name="effectiveLoadDistance"/>, а количество напряжений было не больше максимального количества ТС в автоколонне <paramref name="maxVehiclesInColumn"/>
        /// </summary>
        private VehicleColumnStrain PopulateIndividualColumn(StrainsInTrajectory traj, VehicleRollingSmallModel data, int maxVehicleInTrajectory)
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
        private List<VehicleStrain> GetManyVariants(StrainsInTrajectory traj, VehicleRollingSmallModel data, double effectiveLoadDistance)
        {
            var vehicleStrainList = new List<VehicleStrain>();
            foreach (var vehicleStrain in traj.Strains)
            {
                vehicleStrainList.Add(vehicleStrain);
                var distanceFromExtremum = effectiveLoadDistance;
                bool isValidMax = vehicleStrain.Position + distanceFromExtremum
                    <= data.Surface.MaxY + data.Load.Length;
                bool isValidMin = vehicleStrain.Position - distanceFromExtremum
                    >= data.Surface.MinY - data.Load.Length;
                while (isValidMax || isValidMin)
                {
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

        private bool TryCloneVehicleStrain(StrainsInTrajectory traj, VehicleRollingSmallModel data, VehicleStrain vehicleStrain, double distanceFromExtremum, out VehicleStrain? cloned)
        {
            cloned = CloneVehicleStrain(traj, data, vehicleStrain, distanceFromExtremum);
            if (cloned != null)
            {
                return true;
            }
            return false;
        }

        private VehicleStrain? CloneVehicleStrain(StrainsInTrajectory traj, VehicleRollingSmallModel data, VehicleStrain vehicleStrain, double distanceFromExtremum)
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
            var linkedList = new LinkedList<VehicleStrain>();
            var orderedLinkedList = new SortedList<double, LinkedListNode<VehicleStrain>>(allVehicleStrains.OrderBy(x => x.Position).Select(linkedList.AddLast).ToDictionary(x => -x.Value.TotalStrain));
            var listKeys = orderedLinkedList.Keys.ToArray();

            var resultStrains = new List<VehicleStrain>();

            foreach (var key in listKeys)
            {
                if (linkedList.Count == 0 || resultStrains.Count >= maxVehiclesInColumn)
                {
                    break;
                }
                if (orderedLinkedList.ContainsKey(key) && orderedLinkedList[key].List == linkedList)
                {
                    resultStrains.Add(orderedLinkedList[key].Value);
                    RemoveNodesRecursively(
                        linkedList,
                        orderedLinkedList[key],
                        orderedLinkedList[key].Value.Position,
                        effectiveLoadDistance);
                }
            }

            return resultStrains;
        }

        /// <summary>
        /// Оптимизированное рекурсивное исключение напряжений. При повторном вызове связный список уже не будет содержать исключённые напряжения
        /// </summary>
        private void RemoveNodesRecursively(
            LinkedList<VehicleStrain> list,
            LinkedListNode<VehicleStrain> linkedListNode,
            double centerPos,
            double radius)
        {
            bool conditionNext = centerPos + radius >= linkedListNode.Value.Position;
            bool conditionPrev = centerPos - radius <= linkedListNode.Value.Position;

            if (linkedListNode.Previous != null)
            {
                RemovePrevNode(list, linkedListNode.Previous, centerPos, radius);
            }
            if (linkedListNode.Next != null)
            {
                RemoveNextNode(list, linkedListNode.Next, centerPos, radius);
            }
            list.Remove(linkedListNode.Value);

            void RemovePrevNode(LinkedList<VehicleStrain> list,
                LinkedListNode<VehicleStrain> node,
                double position,
                double effectiveLoadDistance)
            {
                if (conditionPrev)
                {
                    if (node.Previous != null)
                    {
                        RemovePrevNode(list, node.Previous, position, effectiveLoadDistance);
                    }
                    list.Remove(node.Value);
                }
            }

            void RemoveNextNode(LinkedList<VehicleStrain> list,
                LinkedListNode<VehicleStrain> node,
                double position,
                double effectiveLoadDistance)
            {
                if (conditionNext)
                {
                    if (node.Next != null)
                    {
                        RemoveNextNode(list, node.Next, position, effectiveLoadDistance);
                    }
                    list.Remove(node.Value);
                }
            }
        }
    }
}
