using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients
{
    public class CoefficientProviderFactory(IList<ICoefficientProvider> providers) : ICoefficientProviderFactory
    {
        public ICoefficientProvider GetStrainProvider(SnipEnum snip, LoadGroupTypeEnum loadType) =>
            providers.Single(x => x.WorksInSnips.Contains(snip) && x.WorksForLoads.Contains(loadType));
    }
}
