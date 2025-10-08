using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Models.Algorithmic;
using Abdm.Calculation.BLL.Models.Parameters;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainCalculator
    {
        IEnumerable<StrainResult> GetStrainResult(PassTypeDataModel calculationData, RoadRule[] roadRules);
    }
}