using System;
using g4;

namespace Abdm.Calculation.Graphics
{
    public static class SmoothPointsFactory
    {
        /// <summary>
        /// Возвращает результат сглаживания по гауссу
        /// Нахождение экстремумов по оси Z вдоль оси Y.
        /// точки в пространстве, где находится экстремум
        /// </summary>
        public static SmoothPoints BuildByZ(Vector3d[] vectors)
        {
            var result = new SmoothPoints();
            if (vectors.Length < 3) return result;

            var plateStart = Double.NaN;
            double previousDZ = vectors[1].z - vectors[0].z;

            for (int i = 1; i < vectors.Length - 1; i++)
            {
                var v1 = vectors[i];
                var v2 = vectors[i + 1];
                double dZ = v2.z - v1.z;
                if (previousDZ > 0 && dZ <= 0)
                {
                    if (dZ == 0)
                        plateStart = v1.y;
                    else
                    {
                        var extreme = new Vector3d(
                            v1.x,
                            v1.y,
                            GetOrdinat(v1.yz, v2.yz, v1.y));
                        result.Add(
                            extreme,
                            GetGrads(v1.yz, v2.yz, extreme.yz)
                            );
                    }
                }

                if (previousDZ == 0 && dZ < 0 && !Double.IsNaN(plateStart))
                {
                    var extreme = new Vector3d(
                            v1.x,
                            v1.y + plateStart / 2,
                            GetOrdinat(v1.yz, v2.yz, v1.y + plateStart / 2));
                    result.Add(
                        extreme,
                        GetGrads(v1.yz, v2.yz, extreme.yz)
                        );
                    plateStart = Double.NaN;
                }
                previousDZ = dZ;
            }
            return result;
        }

        /// <summary>
        /// возвращает ординату по значению на оси абсцисс
        /// </summary>
        private static double GetOrdinat(Vector2d v1, Vector2d v2, double X)
            => (X - v1.x) * (v2.y - v1.y) / (v2.x - v1.x) + v1.y;

        /// <summary>
        /// Возвращает углы направлиния от середины до соседних точек
        /// </summary>
        private static double[] GetGrads(Vector2d v1, Vector2d v2, Vector2d middle)
        {
            var result = new double[2];
            var dY = middle.y - v1.y;
            result[0] = Math.Abs(dY) > 0.0001 ? Math.Atan(Math.Abs(v1.x - middle.x) / dY) / Math.PI * 180 : 90;
            dY = v2.y - middle.y;
            result[1] = Math.Abs(dY) > 0.0001 ? Math.Atan(Math.Abs(v2.x - middle.x) / dY) / Math.PI * 180 : 90;
            return result;
        }
    }
}
