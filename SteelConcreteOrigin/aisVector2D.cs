using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AisMaths
{
    [Serializable]
    [DataContract]
    public class aisVector2D : aisPoint
    {


 

        public aisVector2D() { X = 0; Y = 0; PointingError = 0; }
        public aisVector2D(aisPoint point) { X = point.X; Y = point.Y; PointingError = point.PointingError; }


#warning Присвоение полю Х значения, не округленного до 4х знаков
        public aisVector2D(Double x, Double y) { X = x; Y = y; PointingError = 0; }
        public aisVector2D(Double x, Double y, Double pe) { X = Math.Round(x, 4); Y = y; PointingError = pe; }

        public void Set(double x, double y) { X = x; Y = y; }

        public override string ToString() { return String.Format("{0:F4};{1:F6}", X, Y); }

        public override bool Equals(object obj)
        {
            return Math.Abs((obj as aisPoint).X - this.X) < 0.00001 &&
                Math.Abs((obj as aisPoint).Y - this.Y) < 0.0000001;
        }
        public override int GetHashCode() { return base.GetHashCode(); }


        #region Static members
        public static implicit operator System.Drawing.Point(aisVector2D vec)
        {
            return new System.Drawing.Point(vec.Xi, vec.Yi);
        }

 
        public static aisVector2D Empty
        {
            get { return new aisVector2D() { X = Double.NaN, Y = Double.NaN }; }
        }

        public static aisVector2D FromString(String value)
        {
            String bValue = value.Trim();
            if (bValue.Length == 0) return null;
            value = value.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            value = value.Replace(",", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            String[] values = value.Split(new char[] { ';', '\t' });
            Boolean errors = false;
            aisVector2D vertex = new aisVector2D(0, 0);

            if (values.Length == 2)
            {
                Double x; errors |= !Double.TryParse(values[0], out x); vertex.X = x;
                Double y; errors |= !Double.TryParse(values[1], out y); vertex.Y = y;
            }
            else return null;

            if (errors) return null;
            else return vertex;
        }

        public static aisVector2D operator +(aisVector2D a, aisVector2D b) => new aisVector2D(a.X + b.X, a.Y + b.Y);
        public static aisVector2D operator -(aisVector2D a) => new aisVector2D(-a.X, a.Y);
        public static aisVector2D operator -(aisVector2D a, aisVector2D b) => new aisVector2D(a.X - b.X, a.Y - b.Y);
        public static aisVector2D operator *(aisVector2D a, int b) => new aisVector2D(a.X * b, a.Y * b);
        public static aisVector2D operator *(aisVector2D a, double b) => new aisVector2D(a.X * b, a.Y * b);
        public static aisVector2D operator /(aisVector2D a, int b) => new aisVector2D(a.X / b, a.Y / b);
        public static aisVector2D operator /(aisVector2D a, double b) => new aisVector2D(a.X / b, a.Y / b);

        #endregion         



        public double Length { get { return Math.Sqrt(X * X + Y * Y); } }

        public static double AngleBetween(aisVector2D v1, aisVector2D v2)
        {
            return Math.Acos(DotProduct(v1, v2) / (v1.Length * v2.Length));
        }

        private static double DotProduct(aisVector2D v1, aisVector2D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

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

        public void Normalize()
        {
            Double l = Length;
            X /= l;
            Y /= l;
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

        private static void LineCanonicalCoef(double cx, double cy, double ex, double ey, out double ap, out double bp, out double cp)
        {
            ap = ey - cy;
            bp = cx - ex;
            cp = -cx * (ey - cy) + cy * (ex - cx);
        }



        private static double VM(double ax, double ay, double bx, double by) => ax * by - bx * ay;

        public void Negate() { X = -X; Y = -Y; }

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

        internal void Rotate(double β)
        {
            Double x2 = Math.Cos(β) * X - Math.Sin(β) * Y;
            Double y2 = Math.Sin(β) * X + Math.Cos(β) * Y;
            X = x2; Y = y2;
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
    }


    [Serializable]
    [DataContract]
    public class aisPoint
    {
        public Boolean IsEmpty { get { return Double.IsNaN(X) || Double.IsNaN(Y); } }

        public Double X { get { return x; } set { x = Math.Round(value, 4); } }
        public float Xf => (float)X;
        [DataMember]
        private Double x = 0;
        public Double Y { get { return y; } set { y = value; } }
        public float Yf => (float)Y;
        [DataMember]
        private Double y = 0;


        public int Xi => (int)X;
        public int Yi => (int)Y;

        public Double PointingError { get; set; }
        public static aisPoint Zero { get => new aisPoint() { x = 0, y = 0 }; }

        public aisPoint() { X = 0; Y = 0; PointingError = 0; }
        public aisPoint(Double x, Double y) { X = x; Y = y; PointingError = 0; }
        public aisPoint(float x, float y) { X = x; Y = y; PointingError = 0; }
        public override string ToString() => $"x:{X:f3}, y:{Y:f3}";

    }
}
