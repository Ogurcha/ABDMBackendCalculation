using Abdm.Calculation.Graphics.Models;
using Abdm.Calculation.Maths.Helpers;
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


        public static double GetZValueByY(this ProfileYZ profile, double pointY)
        {
            (Vector3d v1, Vector3d v2) = Formulas.FindBetweenValues(profile.Vectors, pointY);
            return GetOrdinat(v1.yz, v2.yz, pointY);
        }
    }
}
