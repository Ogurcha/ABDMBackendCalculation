using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Models.StrainAnalysis.SteelConcrete;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ISteelConcreteOriginFacade
    {
        AnalysisSteelConcrete Analyse(StrainResult strainResult, SteelConcreteData steelConcreteData);
    }
}