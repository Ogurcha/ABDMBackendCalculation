using Abdm.Calculation.SteelConcrete.Enums;
using Abdm.Calculation.SteelConcrete.Helpers;
using Abdm.Calculation.SteelConcrete.Models;
using Abdm.Calculation.SteelConcrete.SteelConcrete;

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


        private double Zs1_s() => Ss_aa() / As();

        private double Is_aa() => Rectangles.Where(x => x.Material == MaterialEnum.Steel).Sum(Iaa) + Corners.Sum(Iaa);

        private double Ir_aa() => Rectangles.Where(x => x.Material == MaterialEnum.Concrete).Sum(Ir_aa);

        private double Is_co() => Rectangles.Where(x => x.Material == MaterialEnum.Steel).Sum(Ico) + Corners.Sum(Ico);

        private double As() => Rectangles.Where(x => x.Material == MaterialEnum.Steel).Sum(A) + Corners.Sum(A);

        private double Ss_aa() => Rectangles.Where(x => x.Material == MaterialEnum.Steel).Sum(Saa) + Corners.Sum(Saa);

        private double Sr_aa() => Rectangles.Where(x => x.Material == MaterialEnum.Concrete).Sum(Sr_aa);

        private double Ico(Rectangle rectangle) =>
            rectangle.Width 
            * 1e-4 
            * Math.Pow(Height2(rectangle) * 1e-4, 3) 
            / 12 
            / (rectangle.Material == MaterialEnum.Concrete ? Nb : 1);

        private double Iaa(Rectangle rectangle) => Saa(rectangle) * Z(rectangle);

        

        private double Ir_aa(Rectangle rectangle) => 
            Sr_aa(rectangle) * 
            (Z(rectangle) - Height2(rectangle) * 2e-4 + rectangle.dYr * 1e-4);

        private double Sr_aa(Rectangle rectangle) => 
            rectangle.Material == MaterialEnum.Concrete 
            ? rectangle.Ar * (Z(rectangle) - Height2(rectangle) * 2e-4 + rectangle.dYr / 1000.0) 
            : 0;

        private double Saa(Rectangle rectangle) => A(rectangle) * Z(rectangle);

        private double Height2(Rectangle rectangle) => rectangle.Height - rectangle.DHeight;
        
        private double A(Rectangle rectangle) =>
            rectangle.Width
            * Height2(rectangle)
            * 1e-6
            / (rectangle.Material == MaterialEnum.Concrete ? Nb : 1);

        private double Z(Rectangle rectangle)
        {
            var z = rectangle.DHeight + Height2(rectangle) / 2;
            foreach (var rect in Rectangles)
            {
                if (rect.Equals(rectangle)) break;
                z += rect.Height;
            }
            return z * 1e-4;
        }

        private Rectangle? VerticalItem => Rectangles.Where(x => x.Material == MaterialEnum.Steel && x.IsVertical()).FirstOrDefault();

        public double Zb_stb { get; internal set; }

        private double Iaa(Corner corner)
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

        private double Saa(Corner corner)
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

        private double Ico(Corner corner) => 
            (corner.Width 
            * 1e-4 
            * Math.Pow(corner.H2 * 1e-4, 3) 
            + (corner.Height - corner.H2) 
            * 1e-4 
            * Math.Pow(corner.H2 * 1e-4, 3)) 
            / 6;

        private double A(Corner corner) => 
            2 * (corner.Width * corner.H2 
            + (corner.Height - corner.H2) * corner.H2) * 1e-6;
    }
}
