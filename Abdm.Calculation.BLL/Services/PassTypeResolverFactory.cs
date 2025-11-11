using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services
{
    public class PassTypeResolverFactory(IList<IPassTypeResolver> resolvers) : IPassTypeResolverFactory
    {
        public IPassTypeResolver? GetPassTypeResolver(StrainCalculationGroupTypeEnum strainCalculationType)
            => resolvers.LastOrDefault(s => s.StrainCalculationTypes.Contains(strainCalculationType));
    }
}
