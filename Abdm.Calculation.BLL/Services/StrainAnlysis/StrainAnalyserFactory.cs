using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis
{
    /// <summary>
    /// фэктори, возвращающий анализатор напряжения в зависимости от типа группы расчета напряжения.
    /// </summary>
    public class StrainAnalyserFactory
        (List<IAnalysisWriter> strategies) : IStrainAnalyserFactory
    {
        public IAnalysisWriter GetStrainAnalyser(StrainCalculationGroupTypeEnum strainCalculationGroupType)
            => strategies.First(s => s.StrainCalculationGroupTypes.Contains(strainCalculationGroupType));
    }
}
