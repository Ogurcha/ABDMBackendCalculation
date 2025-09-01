using Abdm.Calculation.Models;

namespace Abdm.Calculation.BusinessLogic
{
    public interface IRoadRulesManager
    {
        RoadRules RoadRule { get; }
        
        RoadRules RefreshRoadRules(long issoId, NagruzkaEnum nagruzkaType);
        
        bool HasSecondaryRule { get; }

        RoadRules SecondaryRoadRule { get; }
    }
}