using System;
using System.Collections.Generic;
using g4;
using static Abdm.Calculation.Extensions.GeometryExtensions;

namespace Abdm.Calculation.Graphics
{
    public static class SmoothPointsFactory
    {
        /// <summary>
        /// Возвращает результат сглаживания по гауссу
        /// Нахождение экстремумов по оси Z вдоль оси Y.
        /// точки в пространстве, где находится экстремум
        /// </summary>
        /// <param name="vectors">Отсортированный список векторов по Y</param>
        /// <returns></returns>
        public static SmoothPoints Create(Vector3d[] vectors)
        {
            var result = new SmoothPoints();
            var extremeList = new List<Vector3d>();

            if (vectors.Length < 3)
            {
                return result;
            }

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
                    {
                        plateStart = v1.y;
                    }
                    else
                    {
                        var extreme = new Vector3d(
                            v1.x,
                            v1.y,
                            GetOrdinat(v1.yz, v2.yz, v1.y));

                        extremeList.Add(extreme);
                    }
                }

                if (previousDZ == 0 && dZ < 0 && !Double.IsNaN(plateStart))
                {
                    var extreme = new Vector3d(
                            v1.x,
                            v1.y + plateStart / 2,
                            GetOrdinat(v1.yz, v2.yz, v1.y + plateStart / 2));

                    extremeList.Add(extreme);

                    plateStart = Double.NaN;
                }
                previousDZ = dZ;
            }
            result.Points = extremeList.ToArray();
            return result;
        }
    }
}
