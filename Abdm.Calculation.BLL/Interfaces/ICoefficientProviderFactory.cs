using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ICoefficientProviderFactory
    {
        ICoefficientProvider GetStrainProvider(SnipEnum snip, LoadGroupTypeEnum loadType);
    }
}