using Abdm.Calculation.Graphics.Models;
using Abdm.Calculation.Maths.Models;
using g4;

namespace Abdm.Calculation.Graphics
{
    public interface IMeshManager
    {
        /// <summary>
        /// возврает меш по массиву точек
        /// </summary>
        Mesh GetMeshFromPoints(Vector3D[] points, Vector3I[] triangleList, bool mirrorZ = false);

        /// <summary>
        /// Возвращает результат пересечения поверхности с плоскостью, параллельной плоскости YZ
        /// </summary>
        IEnumerable<Vector3d>? GetIntersectionVectors(Mesh mesh, double X);
    }
}