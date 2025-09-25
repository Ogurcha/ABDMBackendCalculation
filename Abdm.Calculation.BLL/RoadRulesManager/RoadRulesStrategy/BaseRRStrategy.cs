using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy
{
    public abstract class BaseRRStrategy
    {
        public abstract RoadRules GetRoadRules();

        public abstract List<LoadEnum> LoadIds { get; }

        protected RoadRules Merge(RoadRules value, RoadRules valueSecondary)
        {
            return new RoadRules
            {
                IsPedestrianAllowed = value.IsPedestrianAllowed || valueSecondary.IsPedestrianAllowed,
                IsDynamicMovement = value.IsDynamicMovement || valueSecondary.IsDynamicMovement,
                HasSafetyLine = value.HasSafetyLine && valueSecondary.HasSafetyLine,
                MaxAutoInColumn = Math.Max(value.MaxAutoInColumn, valueSecondary.MaxAutoInColumn),
                MaxColumnCount = Math.Max(value.MaxColumnCount, valueSecondary.MaxColumnCount),
                MinColumnDistance = Math.Min(value.MinColumnDistance, valueSecondary.MinColumnDistance)
            };
        }
    }
}
