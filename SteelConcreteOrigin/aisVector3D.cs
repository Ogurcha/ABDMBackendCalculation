using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AisMaths
{
    [DataContract]
    internal class aisVector3D
    {
        public Boolean IsEmpty { get { return Double.IsNaN(X) || Double.IsNaN(Y) || Double.IsNaN(Z); } }

        public Double X { get { return x; } set { x = Math.Round(value, 4); } }
        [DataMember]
        private Double x = 0;
        public Double Y { get { return y; } set { y = Math.Round(value, 4); } }
        [DataMember]
        private Double y = 0;
        public Double Z { get { return z; } set { z = value; } }
        [DataMember]
        private Double z = 0;

        #region Координаты

        public float Xf { get { return (float)X; } }
        public float Yf { get { return (float)Y; } }
        public float Zf { get { return (float)Z; } }

        public int Xi { get { return Convert.ToInt32(X); } }
        public int Yi { get { return Convert.ToInt32(Y); } }
        public int Zi { get { return Convert.ToInt32(Z); } }

        #endregion

        public aisVector3D() { X = 0; Y = 0; Z = 0; }
        public aisVector3D(Double x, Double y, Double z) { X = Math.Round(x, 4); Y = Math.Round(y, 4); Z = z; }

        public aisVector3D(aisPoint point) : this(point.X, point.Y, 0.0) { }

        #region Static members
        public static aisVector3D Empty
        {
            get { return new aisVector3D() { X = Double.NaN, Y = Double.NaN, Z = Double.NaN }; }
        }
        public static aisVector3D FromString(String value)
        {
            String bValue = value.Trim();
            if (bValue.Length == 0) return null;
            value = value.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            value = value.Replace(",", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            String[] values = value.Split(new char[] { ';', '\t' });
            Boolean errors = false;
            aisVector3D vertex = new aisVector3D(0, 0, 0);

            if (values.Length == 3)
            {
                Double x; errors |= !Double.TryParse(values[0], out x); vertex.X = x;
                Double y; errors |= !Double.TryParse(values[1], out y); vertex.Y = y;
                Double z; errors |= !Double.TryParse(values[2], out z); vertex.Z = z;
            }
            else return null;
            if (errors) return null;
            else return vertex;
        }
        public static Boolean IsPointsEqual2D(aisVector3D point1, aisVector3D point2)
        {
            return Math.Abs(point1.X - point2.X) < 0.0001 &&
                    Math.Abs(point1.Y - point2.Y) < 0.0001;
        }

        public static aisVector3D operator -(aisVector3D v)
        {
            return new aisVector3D(-v.x, -v.y, -v.z);
        }

        public static aisVector3D operator -(aisVector3D left, aisVector3D right)
        {
            return new aisVector3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static aisVector3D operator +(aisVector3D left, aisVector3D right)
        {
            return new aisVector3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static aisVector3D operator *(aisVector3D vec, double k)
        {
            return new aisVector3D(vec.X * k, vec.Y * k, vec.Z * k);
        }

        public static aisVector3D operator *(double k, aisVector3D vec)
        {
            return new aisVector3D(vec.X * k, vec.Y * k, vec.Z * k);
        }

        public static implicit operator aisVector2D(aisVector3D v)
        {
            return new aisVector2D(v.X, v.Y);
        }

        #endregion


        public aisVector3D crossProduct(aisVector3D right)
        {
            return new aisVector3D(Y * right.Z - Z * right.Yf, Z * right.X - X * right.Z,
                X * right.Y - Y * right.X);
        }

        public void set(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public void Normalize()
        {
            double length = Math.Sqrt(X * X + Y * Y + Z * Z);

            if (length < 1e-15)			      
                return;

            double scalefactor = 1.0 / length;
            x *= scalefactor;
            y *= scalefactor;
            z *= scalefactor;
        }
        public override string ToString() { return String.Format("{0:F4};{1:F4};{2:F6}", X, Y, Z); }

        public Boolean Equals2D(aisVector3D point) { return aisVector3D.IsPointsEqual2D(this, point); }
        public override bool Equals(object obj)
        {
            return obj is aisVector3D &&
                Math.Abs((obj as aisVector3D).X - this.X) < 0.00001 &&
                Math.Abs((obj as aisVector3D).Y - this.Y) < 0.00001 &&
                Math.Abs((obj as aisVector3D).Z - this.Z) < 0.0000001;
        }
        public override int GetHashCode() { return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode(); }

        public double Length { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }

        public aisVector3D GetRotated(aisVector3D axis, double angle)
        {
            if (angle == 0.0)
                return new aisVector3D(x, y, z);

            aisVector3D u = axis.GetNormalized();

            aisVector3D rotMatrixRow0 = new aisVector3D(),
                rotMatrixRow1 = new aisVector3D(),
                rotMatrixRow2 = new aisVector3D();

            float sinAngle = (float)Math.Sin(Math.PI * angle / 180);
            float cosAngle = (float)Math.Cos(Math.PI * angle / 180);
            float oneMinusCosAngle = 1.0f - cosAngle;

            rotMatrixRow0.x = (u.x) * (u.x) + cosAngle * (1 - (u.x) * (u.x));
            rotMatrixRow0.y = (u.x) * (u.y) * (oneMinusCosAngle) - sinAngle * u.z;
            rotMatrixRow0.z = (u.x) * (u.z) * (oneMinusCosAngle) + sinAngle * u.y;

            rotMatrixRow1.x = (u.x) * (u.y) * (oneMinusCosAngle) + sinAngle * u.z;
            rotMatrixRow1.y = (u.y) * (u.y) + cosAngle * (1 - (u.y) * (u.y));
            rotMatrixRow1.z = (u.y) * (u.z) * (oneMinusCosAngle) - sinAngle * u.x;

            rotMatrixRow2.x = (u.x) * (u.z) * (oneMinusCosAngle) - sinAngle * u.y;
            rotMatrixRow2.y = (u.y) * (u.z) * (oneMinusCosAngle) + sinAngle * u.x;
            rotMatrixRow2.z = (u.z) * (u.z) + cosAngle * (1 - (u.z) * (u.z));

            return new aisVector3D(this.DotProduct(rotMatrixRow0),
                                this.DotProduct(rotMatrixRow1),
                                this.DotProduct(rotMatrixRow2));
        }

        public aisVector3D GetNormalized()
        {
            aisVector3D res = new aisVector3D(x, y, z);
            res.Normalize();
            return res;
        }

        public double DotProduct(aisVector3D rhs)
        {
            return x * rhs.x + y * rhs.y + z * rhs.z;
        }

        public void Nullify()
        {
            x = y = z = 0.0;
        }

        public aisVector3D Multiply(double p)
        {
            return this * p;
        }

        public aisVector2D To2D()
        {
            return new aisVector2D(this.X, this.Y);
        }
    }
}
