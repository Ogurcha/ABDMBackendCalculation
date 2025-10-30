using Abdm.Calculation.SteelConcrete.Enums;
using Abdm.Calculation.SteelConcrete.Helpers;
using Abdm.Calculation.SteelConcrete.Models;

namespace Abdm.Calculation.SteelConcrete
{
    public class SteelConcretePassChecker : ISteelConcretePassChecker
    {
        public SteelConcretePassCheckResultEnum CheckPass(
            double strain,
            double pedestrianWeight,
            CrossSection crossSection,
            IssoSteelConcreteParameters parameters)
        {
            var m1 = Formulas.ConvertTonnMeterTokNm(parameters.M1);
            var m2 = Formulas.ConvertTonnMeterTokNm(parameters.M2g);
            var strainWithoutPedestrian = Formulas.ConvertTonnMeterTokNm(strain);
            var strainWithPedestrian = Formulas.ConvertTonnMeterTokNm(strain + pedestrianWeight);
            var nb1 = Formulas.Nb(parameters.Eb, parameters.Es, false);
            var nb2 = Formulas.Nb(parameters.Eb, parameters.Es, true);
            var CS = new CrossSectionCalculator
            {
                Rectangles = crossSection.Rectangles,
                Corners = crossSection.Corners,
                Nb = nb1
            };

            var ib = GetIb(crossSection, nb1);
            var ist = CS.Ist();

            if (parameters.Eb * ib > 0.2 * parameters.Es * CS.Is())
            {
                return SteelConcretePassCheckResultEnum.CannotUseSteelConcreteCheck;
            }

            var sigmaB1 = m2 / (nb1 * CS.Zb_stb);
            var shouldCalcualteCreepEffects = sigmaB1 > 0.2 * parameters.Rb;
        }

        private static double GetIb(CrossSection crossSection, double nb1)
        {
            var concreteCalculator = new CrossSectionCalculator
            {
                Rectangles = crossSection.Rectangles.Where(x => x.Material == MaterialEnum.Concrete).ToArray(),
                Corners = [],
                Nb = nb1
            };
            return concreteCalculator.Ib();
        }
    }

    
}
