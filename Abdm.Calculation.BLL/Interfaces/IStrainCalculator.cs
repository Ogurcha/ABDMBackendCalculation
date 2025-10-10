using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainCalculator
    {
        PassTypeEnum GetPassType(PassTypeSmallModel data, List<IntervalModel> intervalModels, RoadRule[] roadRules);
    }
}