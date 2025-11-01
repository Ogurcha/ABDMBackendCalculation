using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.SteelConcrete.Helpers
{
    internal static class Constants
    {
        internal const double Gravity = 9.81;

        internal const double ksi1 = 1;

        internal const double ksi2 = 1;

        internal const double ksi4 = 1;

        internal const double MaxKsi3 = 1.3;

        internal const double Ksi3Base5 = -4.92770e-10;

        internal const double Ksi3Base4 = 5.54609E-8;

        internal const double Ksi3Base3 = 1.95547E-6;

        internal const double Ksi3Base2 = 5.10810E-4;

        internal const double Ksi3Base1 = 0.0300060;

        internal const double Ksi3Base0 = 0.510542;

        internal const double EpsilonLimit = 1e-3;


        internal const double ShrinkageCombined = 1e-4;

        internal const double ShrinkageSeparate = 2e-4;




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
