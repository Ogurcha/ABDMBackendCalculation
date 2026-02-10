using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainResultService
    {
        List<StrainResult> GetStrainResults(PassTypeSmallModel data, IEnumerable<IntervalModel> intervals, IEnumerable<RoadRule> rules, Mesh mesh);
    }
}