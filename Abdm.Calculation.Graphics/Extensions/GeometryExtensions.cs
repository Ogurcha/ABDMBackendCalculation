using Abdm.Calculation.Graphics.Models;
using g4;

namespace Abdm.Calculation.Graphics.Extensions
{
    public static class GeometryExtensions
    {
        /// <summary>
        /// возвращает ординату по значению на оси абсцисс
        /// </summary>
        public static double GetOrdinat(Vector2d v1, Vector2d v2, double X)
            => (X - v1.x) * (v2.y - v1.y) / (v2.x - v1.x) + v1.y;


        public static double GetZ(this SmoothPoints points, double pointY)
        {
            for (int i = 0; i < points.Points.Length; i++)
            {
                var p = points.Points[i];
                if (p.y == pointY)
                {
                    return p.z;
                }

                if (p.y > pointY && i > 0)
                {
                    return points.Points[i - 1].z;
                }
            }

            return points.Points[points.Points.Length - 1].z;
        }
    }
}
