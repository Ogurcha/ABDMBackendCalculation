using Abdm.Calculation.Models;

namespace Abdm.Calculation.RoadRules
{
    public interface IRoadRulesManager
    {
        RoadRules RoadRule { get; }
        
        RoadRules RefreshRoadRules(long issoId, LadingEnum ladingId);
    }
}