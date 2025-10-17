using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ICalculationCoordinator
    {
        PassTypeEnum GetPassType(PassTypeSmallModel data, IEnumerable<IntervalModel> intervals, IEnumerable<RoadRule> rules, Mesh mesh);
    }
}