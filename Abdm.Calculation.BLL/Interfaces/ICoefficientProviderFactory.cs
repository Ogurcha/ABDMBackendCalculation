using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Services.StrainCoefficients;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ICoefficientProviderFactory
    {
        ICoefficientProvider GetStrainProvider(SnipEnum snip, LoadGroupTypeEnum loadType);
    }
}