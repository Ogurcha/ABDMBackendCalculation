using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IRoadRulesManager
    {
        RoadRules RoadRule { get; }
        
        RoadRules RefreshRoadRules(long issoId, LadingEnum ladingId);
    }
}