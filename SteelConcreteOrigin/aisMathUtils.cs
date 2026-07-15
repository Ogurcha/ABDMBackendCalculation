using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisMaths
{
    internal static class aisMathUtils
    {
        public static double GetRectangleIkr(double Width, double Height)
        {
            double a = Math.Max(Width / 2, Height / 2);
            double b_t = Math.Min(Width / 2, Height / 2);
            if (a < 1e-8) return 0.0;
            return a * Math.Pow(b_t, 3) * (16f / 3f - 3.36 * b_t / a * (1 - Math.Pow(b_t, 4) / (12 * Math.Pow(a, 4))));
        }

        public static double PointOrientation(double x1, double y1, double x2, double y2, double xi, double yi)
        {
            double A = y2 - y1;
            double B = x1 - x2;
            double C = x2 * y1 - y2 * x1;
            return A * xi + B * yi + C;
        }
        public static double PointOrientation(aisVector2D p1, aisVector2D p2, aisVector2D pi)
        { return PointOrientation(p1.X, p1.Y, p2.X, p2.Y, pi.X, pi.Y); }


        public static aisVector2D LineIntersectionPoint(aisVector2D p11, aisVector2D p12,
            aisVector2D p21, aisVector2D p22)
        {
            double x1 = p11.X; double y1 = p11.Y; double x2 = p12.X; double y2 = p12.Y;
            double x3 = p21.X; double y3 = p21.Y; double x4 = p22.X; double y4 = p22.Y;
            if ((y2 - y1).IsZero() && (y4 - y3).IsZero())
                return null;
            double k1; double k2;
            if (!(y2 - y1).IsZero())
                k1 = (x2 - x1) / (y2 - y1);
            else
                k1 = double.PositiveInfinity;
            if (!(y4 - y3).IsZero())
                k2 = (x4 - x3) / (y4 - y3);
            else
                k2 = double.PositiveInfinity;
            if ((k1 - k2).IsZero())
                return null;
            double x = ((x1 * y2 - x2 * y1) * (x4 - x3) - (x3 * y4 - x4 * y3) * (x2 - x1)) / ((y1 - y2) * (x4 - x3) - (y3 - y4) * (x2 - x1));
            double y = ((y3 - y4) * x - (x3 * y4 - x4 * y3)) / (x4 - x3);
            return new aisVector2D(x, y);
        }

        public static aisVector2D SegmentIntersectionPoint(aisVector2D p11, aisVector2D p12,
            aisVector2D p21, aisVector2D p22)
        {
            aisVector2D pt = LineIntersectionPoint(p11, p12, p21, p22);
            double x1 = p11.X; double y1 = p11.Y; double x2 = p12.X; double y2 = p12.Y;
            double x3 = p21.X; double y3 = p21.Y; double x4 = p22.X; double y4 = p22.Y;
            if (SegmentIntersectionPoint(x1, y1, x2, y2, x3, y3, x4, y4, out double x, out double y))
                return new aisVector2D(x, y);
            else
                return null;
        }
        public static bool IsZero(this double value, double epsilon = 1e-6) => Math.Abs(value) < epsilon;
        private static double VM(double ax, double ay, double bx, double by) => ax * by - bx * ay;

        public static bool IsLineIntersection(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            double v1 = VM(x4 - x3, y4 - y3, x1 - x3, y1 - y3);
            double v2 = VM(x4 - x3, y4 - y3, x2 - x3, y2 - y3);
            double v3 = VM(x2 - x1, y2 - y1, x3 - x1, y3 - y1);
            double v4 = VM(x2 - x1, y2 - y1, x4 - x1, y4 - y1);
            if ((v1 * v2 <= 0) && (v3 * v4 <= 0)
                &&
                (!(((x1 == x3) && (y1 == y3))
                    || ((x1 == x3) && (y1 == y3))
                    || ((x2 == x3) && (y2 == y3))
                    || ((x1 == x4) && (y1 == y4))
                    || ((x2 == x4) && (y2 == y4))
                ))
                ) return true;
            else return false;
        }

        public static void LineCanonicalCoef(double cx, double cy, double ex, double ey, out double ap, out double bp, out double cp)
        {
            ap = ey - cy;
            bp = cx - ex;
            cp = -cx * (ey - cy) + cy * (ex - cx);
        }

        public static bool SegmentIntersectionPoint(
            double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4,
            out double x, out double y)
        {
            LineCanonicalCoef(x1, y1, x2, y2, out double a1, out double b1, out double c1);
            LineCanonicalCoef(x3, y3, x4, y4, out double a2, out double b2, out double c2);
            double d = a1 * b2 - b1 * a2;
            if (IsLineIntersection(x1, y1, x2, y2, x3, y3, x4, y4))
            {
                double dx = -c1 * b2 + b1 * c2;
                double dy = -a1 * c2 + c1 * a2;
                x = dx / d;
                y = dy / d;
                return true;
            }
            else
            {
                x = double.NaN;
                y = double.NaN;
                return false;
            }
        }

        #region Функции интерполяции
        public static Double Interpol (Double num1, Double res1, Double num3, Double res3, Double num2)
        {
            if (!(num3 - num1).IsZero ())
                return (res3 - res1) / (num3 - num1) * (num2 - num1) + res1;
            else
                return res1;
        }
        public static Double InterpolAr (Double [] ar1, Double [] ar2, Double num)
        {
            int k = 0;
            if (ar1 [0] < ar1 [ar1.Length - 1])
            {
                if (num <= ar1 [0]) { num = ar1 [0]; k = 0; }
                if (num >= ar1 [ar1.Length - 1]) { num = ar1 [ar1.Length - 1]; k = ar1.Length - 2; }
            }
            else
            {
                if (num <= ar1 [ar1.Length - 1]) { num = ar1 [ar1.Length - 1]; k = ar1.Length - 2; }
                if (num >= ar1 [0]) { num = ar1 [0]; k = 0; }
            }
            for (int i = 0; i < ar1.Length - 1; i++)
            {
                if ((num >= Math.Min (ar1 [i], ar1 [i + 1])) && (num < Math.Max (ar1 [i + 1], ar1 [i])))
                { k = i; break; }
            }
            return Interpol (ar1 [k], ar2 [k], ar1 [k + 1], ar2 [k + 1], num);
        }
        public static double InterpolAr2D (double [] ar1AxleY, double [] ar2AxleX, double [,] arValues, double YValue, double XValue)
        {
            double [] y_arr = ar1AxleY;
            double [] x_arr = ar2AxleX;
            double [] ca = new double [y_arr.Length];
            double [] a_var = new double [x_arr.Length];
            for (int j = 0; j < x_arr.Length; j++)
            {
                for (int i = 0; i < y_arr.Length; i++)
                {
                    ca [i] = arValues [i, j];
                }
                a_var [j] = InterpolAr (y_arr, ca, YValue);
            }
            return InterpolAr (x_arr, a_var, XValue);
        }
        #endregion
    }

}
