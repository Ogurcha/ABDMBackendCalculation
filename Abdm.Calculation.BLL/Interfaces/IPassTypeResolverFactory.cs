using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IPassTypeResolverFactory
    {
        IPassTypeResolver? GetPassTypeResolver(StrainCalculationGroupTypeEnum strainCalculationType);
    }
}