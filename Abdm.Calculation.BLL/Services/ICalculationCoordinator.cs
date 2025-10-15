using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services
{
    public interface ICalculationCoordinator
    {
        PassTypeEnum GetPassType(PassTypeSmallModel data, IEnumerable<IntervalModel> intervals, IEnumerable<RoadRule> rules);
    }
}