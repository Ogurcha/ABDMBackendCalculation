using Abdm.Calculation.Graphics.Models;
using g4;

namespace Abdm.Calculation.Graphics
{
    public class MeshManager (IEqualityComparer<double> doubleEqualityComparer) : IMeshManager
    {
        private const int ExtraOne = 1;

        /// <summary>
        /// возврает меш по массиву точек
        /// </summary>
        public Mesh GetMeshFromPoints((double X, double Y, double Z)[] points, (int p1, int p2, int p3)[] triangleList)
        {
            ArgumentNullException.ThrowIfNull(points);

            var mesh = DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(
                points.Select(p => new Vector3d(p.X, p.Y, p.Z)), 
                triangleList.Select(t => new Index3i(t.p1, t.p2, t.p3)));

            var data = UpdateMeshData(mesh);

            var meshAABBTree = new DMeshAABBTree3(mesh, true);

            return new Mesh { Tree = meshAABBTree, Data = data };
        }

        /// <summary>
        /// Возвращает результат пересечения поверхности с плоскостью, параллельной плоскости YZ
        /// </summary>
        public IEnumerable<Vector3d>? GetIntersectionVectors(Mesh mesh, double X)
        {
            var planeYZMesh = GetPlane(mesh, X);
            var planeYZ = new DMeshAABBTree3(planeYZMesh, true);

            var intersection = mesh.Tree.FindAllIntersections(planeYZ);

            if (intersection.Segments.Count == 0)
            {
                return null;
            }

            var pointToPointIntersections = intersection.Points.Select(p => p.point);
            var segmentIntersections = intersection.Segments.Select(s => s.point0).Concat(intersection.Segments.Select(s => s.point1));
            var allPoints = segmentIntersections.Concat(pointToPointIntersections);
            var result = allPoints.DistinctBy(p => p.y, doubleEqualityComparer);

            return result;
        }

        /// <summary>
        /// генерация данных для кэша
        /// </summary>
        private MeshData UpdateMeshData(DMesh3 mesh)
        {
            var result = new MeshData();
            var veticles = mesh.Vertices();

            result.DistinctXs = veticles.Select(v => v.x).Order().Distinct(doubleEqualityComparer).ToArray();
            result.DistinctYs = veticles.Select(v => v.y).Order().Distinct(doubleEqualityComparer).ToArray();

            return result;
        }

        /// <summary>
        /// В библиотеке нет понятия бесконечной плоскости. 
        /// Пробовал создавать <see cref="double.MaxValue"/> размера треугольник, но это по непонятной причине сводило библиотеку с ума. 
        /// Пришлось имитировать плоскость двумя полигонами размером на <see cref="ExtraOne"/> больше, тем целевая поверхность
        /// </summary>
        private DMesh3 GetPlane(Mesh mesh, double X)
        {
            return DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(
                    [
                        new Vector3d(X, mesh.Tree.Bounds.Min.y - ExtraOne, mesh.Tree.Bounds.Min.z - ExtraOne),
                        new Vector3d(X, mesh.Tree.Bounds.Max.y + ExtraOne, mesh.Tree.Bounds.Min.z - ExtraOne),
                        new Vector3d(X, mesh.Tree.Bounds.Min.y - ExtraOne, mesh.Tree.Bounds.Max.z + ExtraOne),
                        new Vector3d(X, mesh.Tree.Bounds.Max.y + ExtraOne, mesh.Tree.Bounds.Max.z + ExtraOne),
                    ],
                    [
                        Index3i,
                        Index3i + 1
                    ]
                );
        }

        /// <summary>
        /// Дефолтные три точки
        /// </summary>
        private static Index3i Index3i => new Index3i(0, 1, 2);
    }
}
