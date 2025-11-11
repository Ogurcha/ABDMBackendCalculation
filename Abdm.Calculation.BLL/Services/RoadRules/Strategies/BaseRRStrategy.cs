using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies
{
    public abstract class BaseRRStrategy
    {
        public abstract RoadRule[] GetRoadRules(LoadEnum loadId);

        public abstract List<LoadGroupTypeEnum> LoadGroupTypes { get; }
    }
}
