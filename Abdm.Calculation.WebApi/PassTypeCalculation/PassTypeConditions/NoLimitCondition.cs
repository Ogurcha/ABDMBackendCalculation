using System;
using System.Collections.Generic;
using System.Linq;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.ColumnCalculation;

namespace Abdm.Calculation.WebApi.PassTypeCalculation.PassTypeConditions
{
    public class NoLimitCondition : IPassTypeCondition
    {
        public bool CanPassCondition(List<ColumnModel> columnList, Surface surface, RoadRules roadRules)
        {
            var totalColumns = Math.Min(roadRules.MaxColumnCount, columnList.Count);

            var dynamicLoad = columnList.Take(totalColumns).Sum(c => c.Strain?.Max());

            dynamicLoad *= PassTypeCalculator.DynamicCoefficient;

            return surface.MyStrength > surface.ConstLoad + surface.PedestrianLoad + surface.OtherLoad + dynamicLoad;
        }
    }
}
