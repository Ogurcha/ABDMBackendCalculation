using Abdm.Calculation.Models;

namespace Abdm.Calculation.BusinessLogic
{
    public interface IRoadRulesManager
    {
        RoadRules RoadRules { get; }

        RoadRules RefreshRoadRules(long issoId, NagruzkaTypeEnum nagruzkaType);
    }
}