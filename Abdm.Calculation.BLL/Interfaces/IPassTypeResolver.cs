using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IPassTypeResolver
    {
        public StrainCalculationGroupTypeEnum[] StrainCalculationTypes { get; }

        PassTypeEnum Resolve(List<StrainResult> strainResults, PassTypeSmallModel data);
    }
}