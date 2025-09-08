using System;
using System.Collections.Generic;
using System.Linq;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.WebApi.PassTypeCalculation.PassTypeConditions
{
    public class Speed10Condition : IPassTypeCondition
    {
        public bool CanPassCondition(List<ColumnModel> columnList, Surface surface, RoadRules roadRules)
        {
            var totalColumns = Math.Min(roadRules.MaxColumnCount, columnList.Count);

            var dynamicLoad = columnList.Take(totalColumns).Sum(c => c.Strain?.Max());

            return surface.MyStrength > surface.ConstLoad + surface.OtherLoad + dynamicLoad;
        }
    }
}
