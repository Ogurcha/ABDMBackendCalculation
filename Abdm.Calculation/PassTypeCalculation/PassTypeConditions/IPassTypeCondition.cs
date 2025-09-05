using System.Collections.Generic;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.WebApi.PassTypeCalculation.PassTypeConditions
{
    public interface IPassTypeCondition
    {
        bool CanPassCondition(List<Column> columnList, Surface surface, RoadRules roadRules);
    }
}
