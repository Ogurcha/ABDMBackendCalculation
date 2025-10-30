using Abdm.Calculation.SteelConcrete.Models;

namespace Abdm.Calculation.SteelConcrete.Helpers
{
    public static class CheckPointTestData
    {
        public static IssoSteelConcreteParameters GetParameters()
        {
            return new IssoSteelConcreteParameters
            {
                Ea = Ea,
                Es = Es,
                Eb = Eb,
                M1 = M1,
                M2g = M2g,
                Mp = Mp,
                Rb = Rb,
            };
        }

        public const double M1 = 129.8;

        public const double M2g = 85.9;

        public const double Mp = 9.2;

        public const double Eb = 36000;

        public const double Es = 206000;

        public const double Ea = 206000;

        public const double Rb = 20;

    }
}
