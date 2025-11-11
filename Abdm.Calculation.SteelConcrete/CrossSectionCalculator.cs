using Abdm.Calculation.SteelConcrete.Enums;
using Abdm.Calculation.SteelConcrete.Helpers;
using Abdm.Calculation.SteelConcrete.Models;
using Abdm.Calculation.SteelConcrete.SteelConcrete;
using Formulas = Abdm.Calculation.SteelConcrete.Helpers.Formulas;
using MathFormulas = Abdm.Calculation.Maths.Helpers.Formulas;

namespace Abdm.Calculation.SteelConcrete
{
    internal class CrossSectionCalculator
    {
        internal required Rectangle[] Rectangles { get; set; }

        internal required Corner[] Corners { get; set; }

        internal required double Nb { get; set; }

        internal double Ib() => Ib_aa() + Ib_co() - Sb_aa() / Ab() * Sb_aa();

        internal double Ib_aa() => Rectangles.Sum(r => Iaa(r) + Ir_aa(r));

        internal double Ib_co() => Rectangles.Sum(Ico);

        internal double Sb_aa() => Rectangles.Sum(Saa) + Rectangles.Sum(Sr_aa);

        internal double Ab() => Rectangles.Sum(A) + Rectangles.Sum(r => r.Ar);

        internal double Ist() => Ir_aa() + Is_aa() + Is_co() - Zs1_s() * (Ss_aa() + Sr_aa());

        internal double Is() => Is_aa() + Is_co() - Zs1_s() * Ss_aa();

        internal double Zbr() => Sb_aa() / Ab();

        internal double Zs1_st() => (Ss_aa() + Sr_aa()) / (As() + Ar());

        internal double Zs1_s() => Ss_aa() / As();

        internal double Is_aa() => Rectangles.Where(x => x.Material == MaterialEnum.Steel).Sum(Iaa) + Corners.Sum(Iaa);

        internal double Ir_aa() => Rectangles.Where(x => x.Material == MaterialEnum.Concrete).Sum(Ir_aa);

        internal double Is_co() => Rectangles.Where(x => x.Material == MaterialEnum.Steel).Sum(Ico) + Corners.Sum(Ico);

        internal double As() => Rectangles.Where(x => x.Material == MaterialEnum.Steel).Sum(A) + Corners.Sum(A);

        internal double Ar() => Rectangles.Where(x => x.Material == MaterialEnum.Concrete).Sum(x => x.Ar);

        internal double Abn() => Ab() - Ar();

        internal double Ss_aa() => Rectangles.Where(x => x.Material == MaterialEnum.Steel).Sum(Saa) + Corners.Sum(Saa);

        internal double Sr_aa() => Rectangles.Where(x => x.Material == MaterialEnum.Concrete).Sum(Sr_aa);

        internal double Ico(Rectangle rectangle) =>
            rectangle.Width 
            * 1e-4 
            * Math.Pow(Height2(rectangle) * 1e-4, 3) 
            / 12 
            / (rectangle.Material == MaterialEnum.Concrete ? Nb : 1);

        internal double Iaa(Rectangle rectangle) => Saa(rectangle) * Z(rectangle);

        

        internal double Ir_aa(Rectangle rectangle) => 
            Sr_aa(rectangle) * 
            (Z(rectangle) - Height2(rectangle) * 2e-4 + rectangle.dYr * 1e-4);

        internal double Sr_aa(Rectangle rectangle) => 
            rectangle.Material == MaterialEnum.Concrete 
            ? rectangle.Ar * (Z(rectangle) - Height2(rectangle) * 2e-4 + rectangle.dYr / 1000.0) 
            : 0;

        internal double Saa(Rectangle rectangle) => A(rectangle) * Z(rectangle);

        internal double Height2(Rectangle rectangle) => rectangle.Height - rectangle.DHeight;
        
        internal double A(Rectangle rectangle) =>
            rectangle.Width
            * Height2(rectangle)
            * 1e-6
            / (rectangle.Material == MaterialEnum.Concrete ? Nb : 1);

        internal double Z(Rectangle rectangle)
        {
            var z = rectangle.DHeight + Height2(rectangle) / 2;
            foreach (var rect in Rectangles)
            {
                if (rect.Equals(rectangle)) break;
                z += rect.Height;
            }
            return z * 1e-4;
        }

        internal Rectangle? VerticalItem => Rectangles.Where(x => x.Material == MaterialEnum.Steel && x.IsVertical()).FirstOrDefault();

        internal double Iaa(Corner corner)
        {
            if (VerticalItem is not Rectangle vi)
            {
                return 0;
            }
            var sign = corner.Location == CornerLocationEnum.Lower ? 1 : -1;
            var sh = corner.Width * corner.H2 * 1e-6;
            var sv = (corner.Height - corner.H2) * corner.H2 * 1e-6;
            var zh = Z(vi) - sign * (vi.Height * 2e-4 - corner.H2 * 2e-4);
            var zv = Z(vi) - sign * (vi.Height * 2e-4 - (corner.H2 + (corner.Height - corner.H2) / 2) * 1e-4);
            return 2 * sh * Math.Pow(zh, 2) + 2 * sv * Math.Pow(zv, 2);
        }

        internal double Saa(Corner corner)
        {
            if (VerticalItem is not Rectangle vi)
            {
                return 0;
            }
            var sign = corner.Location == CornerLocationEnum.Lower ? 1 : -1;
            var sh = corner.Width * corner.H2 * 1e-6;
            var sv = (corner.Height - corner.H2) * corner.H2 * 1e-6;
            var zh = Z(vi) - sign * (vi.Height * 2e-4 - corner.H2 * 2e-4);
            var zv = Z(vi) - sign * (vi.Height * 2e-4 - (corner.H2 + (corner.Height - corner.H2) / 2) * 1e-4);
            return 2 * sh * zh + 2 * sv * zv;
        }

        internal double Ico(Corner corner) => 
            (corner.Width 
            * 1e-4 
            * Math.Pow(corner.H2 * 1e-4, 3) 
            + (corner.Height - corner.H2) 
            * 1e-4 
            * Math.Pow(corner.H2 * 1e-4, 3)) 
            / 6;

        internal double A(Corner corner) => 
            2 * (corner.Width * corner.H2 
            + (corner.Height - corner.H2) * corner.H2) * 1e-6;

        internal double V(double ist) => (Ab() - Ar()) * (1 / Ast() + Math.Pow(Zb_st(), 2) / ist);

        internal double TopBeltWidth() => Rectangles.Where(x => !x.IsVertical() && Z(x) > Z()).FirstOrDefault()?.Width ?? 0.0;

        internal double X()
        {
            var plateItem = GetPlateItem();
            return (plateItem!.Width + Height2(plateItem) - TopBeltWidth()) 
                / (plateItem!.Width * Height2(plateItem))
                * 1e4;
        }

        internal double Ksi3()
        {
            var x = X();
            if (x > 80)
            {
                return Constants.MaxKsi3;
            }
            return
                Constants.Ksi3Base5 * Math.Pow(x, 5)
                * Constants.Ksi3Base4 * Math.Pow(x, 4)
                * Constants.Ksi3Base3 * Math.Pow(x, 3)
                * Constants.Ksi3Base2 * Math.Pow(x, 2)
                * Constants.Ksi3Base1 * x
                * Constants.Ksi3Base0;
        }

        internal double Clim(double cn) =>
            cn
            * Constants.ksi1
            * Constants.ksi2
            * Ksi3()
            * Constants.ksi4;

        internal double GetTetaKr(double Eb) => 1.1 * Eb * Clim(MathFormulas.GetYValueByX(Formulas.GetCNList(), Eb)) * 1e-6;

        internal Rectangle? GetPlateItem() => Rectangles.Where(item => item.Material == MaterialEnum.Concrete).FirstOrDefault();

        internal double Z() => -Zbr() + Zstb_shr();

        internal double Zstb_shr() => Sstb_aa() / Astb();

        internal double Ast() => As() + Ar();

        internal double Zb_st() => Zbr() - Zs1_st(); 

        internal double Wb_stb() => Istb() / Zb_stb();

        internal double SigmaB1(double mass) => mass / (Nb * Wb_stb());

        internal double Istb() => Istb_aa() + Istb_co() - Zstb() * Sstb_aa();

        internal double Zb_stb() => Zbr() - Zstb();

        internal double Zstb() => Sstb_aa() / Astb();

        internal double Astb() => Ab() / As();

        internal double Sstb_aa() => Sb_aa() + Ss_aa();

        internal double Istb_co() => Ib_co() + Is_co();

        internal double Istb_aa() => Ib_aa() + Is_aa();

        internal double GetSigmaBetaShr(double eb) => eb / 2 * (Ast() / Astb() + Sshr() / Istb_shr() * Z());

        internal double GetSigmaAlfaShr(double ea) => ea * (Ast() / Astb() + Sshr() / Istb_shr() * Z() - 1);

        internal double Sshr() => Ast() * Zst_stb();

        internal double Zst_stb() => Zstb_shr() - Zs1_st();

        internal double Istb_shr() => Istb_aa() + Istb_co() - Zstb_shr() * Sstb_aa();

        internal double Wbs() => Is() / Zbs();

        internal double Zbs() => Zbr() - Zs1_s();

        internal double Ws1_s() => Is() - Zs1_s();

        internal double Ws2_s() => Is() - Zs2_s();

        internal double Zs2_s() => hsb() - Zs1_s();

        internal double hsb() => Rectangles.Where(x => x.Material == MaterialEnum.Steel).Sum(x => x.Height) * 1e-4;

        internal double At() => 0.8 * Awt() + 0.3 * As(CornerLocationEnum.Lower);

        internal double Awt() => Rectangles.Where(x => x.Material == MaterialEnum.Steel && x.IsVertical()).Count() > 0
            ? cAwt() 
                + Rectangles.Where(x => x.Material == MaterialEnum.Steel && x.IsVertical()).Sum(A)
                + (Corners.Count() == 2 ? Corners.Sum(Awt) : 0)
            : cAwt();

        internal double cAwt() => Corners.Sum(x => x.Height * 2 - x.H2 * x.H2 * 1e-6);

        internal double As(CornerLocationEnum location)
        {
            Double a = VerticalItem != null && Rectangles.Where(item => item.Material == MaterialEnum.Steel && Z(item) < Z(VerticalItem)).Count() > 0 ?
                   Rectangles.Where(item => item.Material == MaterialEnum.Steel && Z(item) < Z(VerticalItem)).Sum(A) :
                   1e-6;
            return a + Corners.Where(x => x.Location == location).Sum(A);
        }

        internal double Awt(Corner corner) =>
            2
            * (corner.Height - corner.H2)
            * corner.H2
            * 1e-6;

       

    }
}
