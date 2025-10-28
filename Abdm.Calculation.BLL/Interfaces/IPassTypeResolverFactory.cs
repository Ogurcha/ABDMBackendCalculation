using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IPassTypeResolverFactory
    {
        IPassTypeResolver? GetPassTypeResolver(StrainCalculationTypeEnum strainCalculationType);
    }
}