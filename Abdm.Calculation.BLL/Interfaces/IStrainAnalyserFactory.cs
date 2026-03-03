using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainAnalyserFactory
    {
        ISAStrategy GetStrainAnalyser(StrainCalculationGroupTypeEnum strainCalculationGroupType);
    }
}