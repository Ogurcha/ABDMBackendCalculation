using System.Reflection.Metadata;
using System.Security.Cryptography;
using Abdm.Calculation.Maths.Models;
using Abdm.Calculation.SteelConcrete.Enums;
using Abdm.Calculation.SteelConcrete.Helpers;
using Abdm.Calculation.SteelConcrete.Models;
using Formulas = Abdm.Calculation.SteelConcrete.Helpers.Formulas;
using MathFormulas = Abdm.Calculation.Maths.Helpers.Formulas;

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
            var na1 = Formulas.Nb(parameters.Ea, parameters.Es, false);
            var na2 = Formulas.Nb(parameters.Ea, parameters.Es, true);
            var CS = new CrossSectionCalculator
            {
                Rectangles = crossSection.Rectangles,
                Corners = crossSection.Corners,
                Nb = nb1
            };

            ////
            var ib = GetIb(crossSection, nb1);
            if (parameters.Eb * ib > 0.2 * parameters.Es * CS.Is())
            {
                return SteelConcretePassCheckResultEnum.CannotUseSteelConcreteCheck;
            }
            ///////

            ///
            var ist = CS.Ist();
            var v = CS.V(ist);
            var tetaKr = Coalesce(parameters.TetaKrParam, CS.GetTetaKr(parameters.Eb));

            ///
            var tetaKrD = tetaKr + parameters.Eb * parameters.Sd / (0.2 * parameters.Rb * parameters.L);
            var a = tetaKrD / (0.5 * tetaKrD + v + 1);
            var sigmaBetaKr = a * CS.SigmaB1(m2);
            var sigmaAlfaKr = CS.Ar() < 1e-4 
                ? 0
                : a * CS.SigmaB1(m2) * v * nb1;
            ///

            CS.Nb = nb2;

            ///
            var SigmaShr = parameters.PlateType == PlateTypeEnum.Combined 
                ? Constants.ShrinkageCombined
                : Constants.ShrinkageSeparate;

            var sigmaBetaShr = Coalesce(parameters.SigmaBetaShrParam, CS.GetSigmaBetaShr(parameters.Eb) * SigmaShr);
            var sigmaAlfaShr = Coalesce(parameters.SigmaAlfaShrParam, CS.GetSigmaAlfaShr(parameters.Ea) * SigmaShr);
            ///

            CS.Nb = nb1;

            ///
            var sigmaT = GetSigmaT(CS);
            var sigmaBetaT = Coalesce(parameters.SigmaBetaTParam, sigmaT * parameters.Eb * parameters.TMax);
            var sigmaAlfaT = Coalesce(parameters.SigmaAlfaTParam, sigmaT * parameters.Ea * parameters.TMax);
            ///
            var A = CS.As(CornerLocationEnum.Upper) / CS.As(CornerLocationEnum.Lower);

            var results = new List<double>();
            CalculateFirstCombination(strainWithPedestrian + m2);
            CS.Nb = nb2;
            CalculateSecondCombination(0.8 * strainWithPedestrian + m2);
            CS.Nb = nb1;
            if (results.Order().First() > 1)
            {
                return SteelConcretePassCheckResultEnum.CanPass;
            }
            results.Clear();
            CalculateFirstCombination(strainWithoutPedestrian + m2);
            CS.Nb = nb2;
            CalculateSecondCombination(0.8 * strainWithoutPedestrian + m2);
            CS.Nb = nb1;
            if (results.Order().First() > 1)
            {
                return SteelConcretePassCheckResultEnum.CanPassWithoutPedestrianOnly;
            }
            return SteelConcretePassCheckResultEnum.CanNotPass;


            void CalculateFirstCombination(double strain)
            {
                var bigStrain = strain + m1;

                var e70_b70 = CS.SigmaB1(m2) > 0.2 * parameters.Rb;
                var sigmaB = Math.Min(strain / nb1 / CS.Wb_stb() - (e70_b70 ? 1 : 0) * sigmaBetaKr, Rb2(parameters));
                var sigmaR = CS.Ar() > 1e-7
                    ? Math.Min(strain / na1 / CS.Wb_stb() + (e70_b70 ? 1 : 0) * sigmaAlfaKr, parameters.Rr)
                    : 0;

                
                var Nbr = CS.Abn() * sigmaB * nb1 + CS.Ar() * sigmaR;

                var recordContainer = new RecordContainer();
                var n1 = RecordCalculator.GetN(recordContainer, A, Nbr / (parameters.Rs1 * CS.As()), parameters.IsNegative);
                var n2 = RecordCalculator.GetN(recordContainer, A, Nbr / (parameters.Rs2 * CS.As()), parameters.IsNegative);

                var X3Coefficient1 = 1 + n1 * (parameters.X1Coefficient - 1);
                var X3Coefficient2 = 1 + n2 * (parameters.X1Coefficient - 1);

                var M1Coefficient = Math.Min(Math.Max(1, 1 + (Rb2(parameters) - sigmaB) / parameters.Rs2 * nb1 * CS.Abn() / CS.As(CornerLocationEnum.Upper)), 1.2);
                var X4Coefficient = Math.Max(1, X3Coefficient2 / M1Coefficient);

                var vv1 = (bigStrain - CS.Zbs() * Nbr) / (X4Coefficient * CS.Ws2_s()) - Nbr / CS.As();
                var vv2 = (bigStrain - CS.Zbs() * Nbr) / (X3Coefficient1 * CS.Ws1_s()) + Nbr / CS.As();

                results.Add(M1Coefficient * parameters.Rs2 / vv1);
                results.Add(parameters.Rs1 / vv2);

                if (strain / nb1 / CS.Wb_stb() - (e70_b70 ? 1 : 0) * sigmaBetaKr > Rb2(parameters))
                {
                    var K1 = (bigStrain - CS.Zbs() * Nbr) / CS.Ws2_s();
                    var K2 = Rb2(parameters) + Nbr / CS.As();
                    Double K;
                    if (K1 <= K2)
                    {
                        K = 1;
                    }
                    else if (K1 <= K2 * X3Coefficient2)
                    {
                        K = 1 + (K1 - K2) * 1e-3 * parameters.Es / parameters.Rb / (K2 * (X3Coefficient2 - 1));

                    } else
                    {
                        K = 1 + 1e-3 * parameters.Es / parameters.Rb;
                    }

                    results.Add(parameters.EpsilonBetaLim / (parameters.Es * ((strain - CS.Zbs() * Nbr) / CS.Wbs() - Nbr / CS.As())));
                }
            }

            void CalculateSecondCombination(double strain)
            {
                var bigStrain = strain + m1;

                if (CS.SigmaB1(m2) > 0.2 * parameters.Rb)
                {
                    var v = (CS.Ab() - CS.Ar()) * (1 / CS.Ast() + Math.Pow(CS.Zb_st(), 2) / ist);
                    tetaKrD = tetaKr / 2 + parameters.Eb * parameters.Sd / (0.2 * parameters.Rb * parameters.L);
                    a = tetaKrD / (0.5 * tetaKrD + v + 1);
                    sigmaBetaKr = a * CS.SigmaB1(m2);
                    sigmaAlfaKr = a * CS.SigmaB1(m2) * v * nb2;
                }

                var sigmaB = Math.Min(bigStrain / nb2 / CS.Wb_stb() - sigmaBetaKr - sigmaBetaShr - 0.7 * sigmaBetaT, Rb2(parameters));
                var sigmaR = CS.Ar() > 1e-7
                    ? Math.Min(bigStrain / CS.Wb_stb() + sigmaAlfaKr + sigmaAlfaShr + 0.7 * sigmaAlfaT, parameters.Rr)
                    : 0;

                var Nbr = CS.Abn() * sigmaB * nb2 + CS.Ar() * sigmaR;
                var recordContainer = new RecordContainer();
                var n1 = RecordCalculator.GetN(recordContainer, A, Nbr / (parameters.Rs1 * CS.As()), parameters.IsNegative);
                var n2 = RecordCalculator.GetN(recordContainer, A, Nbr / (parameters.Rs2 * CS.As()), parameters.IsNegative);

                var X3Coefficient1 = 1 + n1 * (parameters.X1Coefficient - 1);
                var X3Coefficient2 = 1 + n2 * (parameters.X1Coefficient - 1);
                var M1Coefficient = Math.Min(Math.Max(1, 1 + (Rb2(parameters) - sigmaB) / parameters.Rs2 * CS.Abn() / CS.As(CornerLocationEnum.Upper)), 1.2); //no nb2
                var X4Coefficient = Math.Max(1, X3Coefficient2 / M1Coefficient);

                var vv1 = (bigStrain - CS.Zbs() * Nbr) / (X4Coefficient * CS.Ws2_s()) - Nbr / CS.As();
                var vv2 = (bigStrain - CS.Zbs() * Nbr) / (X3Coefficient1 * CS.Ws1_s()) + Nbr / CS.As();

                results.Add(M1Coefficient * parameters.Rs2 / vv1);
                results.Add(parameters.Rs1 / vv2);

                CS.Nb = nb1;
                if (Math.Truncate(sigmaR - parameters.Rr) == 0)
                {
                    var K1 = (bigStrain - CS.Zbs() * Nbr) / CS.Ws2_s();
                    var K2 = Rb2(parameters) + Nbr / CS.As();
                    Double K;
                    if (K1 <= K2)
                    {
                        K = 1;
                    }
                    else if (K1 <= K2 * X3Coefficient2)
                    {
                        K = 1 + (K1 - K2) * 1e-3 * parameters.Es / parameters.Rb / (K2 * (X3Coefficient2 - 1));

                    }
                    else
                    {
                        K = 1 + 1e-3 * parameters.Es / parameters.Rb;
                    }

                    results.Add(parameters.EpsilonBetaLim / (parameters.Es * ((strain - CS.Zbs() * Nbr) / CS.Wbs() - Nbr / CS.As())));
                }
            }
        }

        private static double Rb2(IssoSteelConcreteParameters parameters) => parameters.Rb * 0.9;

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

        private double Coalesce(double? param, double param2) => (param ?? 0) > Constants.EpsilonLimit ? param!.Value : param2;

        private double GetSigmaT(CrossSectionCalculator CS)
        {
            var at = CS.At();
            var st = 0.4 
                * CS.VerticalItem!.Height 
                * 1e-4 
                - 0.8 
                * CS.Zb_stb() 
                * CS.Awt() 
                + 0.3 
                * CS.As(CornerLocationEnum.Lower) 
                * CS.Zs1_s();
            return 1e-5 * (at / CS.Astb() + st / CS.Istb() * (CS.Zstb() - CS.Zbr()));
        }

        

    }

    
}
