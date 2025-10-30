using Abdm.Calculation.SteelConcrete.Models;

namespace Abdm.Calculation.SteelConcrete.Helpers
{
    internal static class Formulas
    {

        internal static double ConvertTonnMeterTokNm(double value)
        {
            return value * Constants.Gravity * 1e-4;
        }

        internal static double Nb(double Eb, double Es, bool nb1) => Eb > 0 ? (Convert.ToInt16(nb1) + 1) * Es / Eb : 1;

        public static bool IsVertical(this SteelConcreteItem item) => item.Height > item.Width;


    }
}
