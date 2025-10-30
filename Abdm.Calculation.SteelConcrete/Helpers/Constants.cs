namespace Abdm.Calculation.SteelConcrete.Helpers
{
    internal static class Constants
    {
        internal const double Gravity = 9.81;

        internal const double ShrinkageCombined = 1e-4;

        internal const double ShrinkageSeparate = 2e-4;

        internal const double EpsilonLimit = 1e-3;

        internal const double MaxKsi3 = 1.3;

        internal const double InterpolationStep = 0.0009;

        internal const double DefaultShrinkageStress = 0.001;       
    }

    internal static class CrossSectionDefaults
    {
        internal const double Es = 206000;

        internal const double Rs1 = 350;

        internal const double Rs2 = 350;

        internal const double Eb = 32500;

        internal const double Rb = 15.50;

        internal const double ε_b_lim = 0.0016;

        internal const double Ea = 206000;

        internal const double Rr = 350;

        internal const double χ1 = 1;
    }
}
