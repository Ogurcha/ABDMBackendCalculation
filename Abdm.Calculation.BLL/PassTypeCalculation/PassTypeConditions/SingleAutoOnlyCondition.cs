using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.PassTypeCalculation.PassTypeConditions
{
    public class SingleAutoOnlyCondition : IPassTypeCondition
    {
        public bool CanPassCondition(List<ColumnModel> columnList, Surface surface, RoadRules roadRules)
        {
            var totalColumns = Math.Min(roadRules.MaxColumnCount, columnList.Count);

            var dynamicLoad = columnList.Take(totalColumns).Sum(c => c.StrainOneAuto?.Max());

            return surface.MyStrength > surface.ConstLoad + surface.OtherLoad + dynamicLoad;
        }
    }
}
