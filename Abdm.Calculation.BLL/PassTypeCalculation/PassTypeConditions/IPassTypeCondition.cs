using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.PassTypeCalculation.PassTypeConditions
{
    public interface IPassTypeCondition
    {
        bool CanPassCondition(List<ColumnModel> columnList, Surface surface, RoadRules roadRules);
    }
}
