using System.Numerics;
using Abdm.Calculation.Graphics.Models;
using g4;

namespace Abdm.Calculation.Graphics
{
    public interface IMeshManager
    {
        /// <summary>
        /// Возвращает результат сглаживания по гауссу
        /// Нахождение экстремумов по оси Z вдоль оси Y.
        /// точки в пространстве, где находится экстремум
        /// </summary>
        /// <param name="vectors">Отсортированный список векторов по Y</param>
        /// <returns></returns>
        SmoothPoints CreateSmoothPoints(Vector3d[] vectors);

        /// <summary>
        /// возврает меш по массиву точек
        /// </summary>
        Mesh GetMeshFromPoints(Vector3[] points);

        /// <summary>
        /// Возвращает результат пересечения поверхности с плоскостью, параллельной плоскости YZ
        /// </summary>
        IEnumerable<Vector3d> MakeProfileYZ(Mesh mesh, double X);
    }
}