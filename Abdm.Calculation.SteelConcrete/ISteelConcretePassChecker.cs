using Abdm.Calculation.SteelConcrete.Enums;
using Abdm.Calculation.SteelConcrete.Models;

namespace Abdm.Calculation.SteelConcrete
{
    public interface ISteelConcretePassChecker
    {
        SteelConcretePassCheckResultEnum CheckPass(
            double strain,
            double pedestrianWeight,
            CrossSection crossSection,
            IssoSteelConcreteParameters parameters);
    }
}
