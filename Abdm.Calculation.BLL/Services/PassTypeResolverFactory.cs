using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Services
{
    public class PassTypeResolverFactory(IList<IPassTypeResolver> resolvers) : IPassTypeResolverFactory
    {
        public IPassTypeResolver? GetPassTypeResolver(StrainCalculationTypeEnum strainCalculationType)
            => resolvers.LastOrDefault(s => s.StrainCalculationTypes.Contains(strainCalculationType));
    }
}
