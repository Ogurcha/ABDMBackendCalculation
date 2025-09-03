using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRules
{
    public interface IRoadRulesManager
    {
        RoadRules RoadRule { get; }
        
        RoadRules RefreshRoadRules(long issoId, LadingEnum ladingId);
    }
}