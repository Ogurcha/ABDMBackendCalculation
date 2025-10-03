using Abdm.Calculation.Graphics.Models;
using g4;

namespace Abdm.Calculation.Graphics
{
    public interface IMeshManager
    {
        /// <summary>
        /// возврает меш по массиву точек
        /// </summary>
        Mesh GetMeshFromPoints((double X, double Y, double Z)[] points, (int p1, int p2, int p3)[] trianglesList);

        /// <summary>
        /// Возвращает результат пересечения поверхности с плоскостью, параллельной плоскости YZ
        /// </summary>
        IEnumerable<Vector3d>? GetIntersectionVectors(Mesh mesh, double X);
    }
}