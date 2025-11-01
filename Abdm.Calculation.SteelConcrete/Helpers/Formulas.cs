using Abdm.Calculation.Maths.Models;
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

        internal static double Na(double Ea, double Es, bool nb1) => Ea > 0 ? (Convert.ToInt16(nb1) + 1) * Es / Ea : 1;

        internal static bool IsVertical(this SteelConcreteItem item) => item.Height > item.Width;

        internal static SortedList<double, Vector2D> GetCNList()
        {
            var data = new Vector2D[]
            {
                (27000, 115),
                (28500, 107),
                (30000, 100),
                (31500, 92),
                (32500, 84),
                (34500, 75),
                (36000, 67),
                (37500, 55),
                (39000, 50),
                (39500, 41),
                (40000, 39),
            };
            return new SortedList<double, Vector2D>(data.ToDictionary(v => v.X));
        }
    }
}
