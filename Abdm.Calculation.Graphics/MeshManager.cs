using Abdm.Calculation.Graphics.Models;
using g4;
using static Abdm.Calculation.Graphics.Extensions.GeometryExtensions;

namespace Abdm.Calculation.Graphics
{
    public class MeshManager (IEqualityComparer<double> doubleEqualityComparer) : IMeshManager
    {
        private const int smoothingPoint = 2;
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
        public IEnumerable<Vector3d>? MakeProfileYZ(Mesh mesh, double X)
        {
            var planeYZMesh = DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(
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
            var planeYZ = new DMeshAABBTree3(planeYZMesh, true);

            var intersection = mesh.Tree.FindAllIntersections(planeYZ);

            if (intersection.Segments.Count == 0)
            {
                return null;
            }
            var pointToPointintersections = intersection.Points.Select(p => p.point);
            var segmentIntersections = intersection.Segments.Select(s => s.point0).Concat(intersection.Segments.Select(s => s.point1));
            var allPoints = segmentIntersections.Concat(pointToPointintersections);
            var result = allPoints.DistinctBy(p => p.y, doubleEqualityComparer).OrderBy(p => p.y);

            return result;
        }

        /// <summary>
        /// Возвращает результат сглаживания по гауссу
        /// Нахождение экстремумов по оси Z вдоль оси Y.
        /// точки в пространстве, где находится экстремум
        /// </summary>
        /// <param name="vectors">Отсортированный список векторов по Y</param>
        /// <returns></returns>
        public SmoothPoints CreateSmoothPoints(Vector3d[] vectors)
        {
            var extremeList = new List<Vector3d>();

            double? plateStart = null;
            double previousDeltaZ = vectors[1].z - vectors[0].z;

            for (int i = 1; i < vectors.Length - 1; i++)
            {
                var v1 = vectors[i];
                var v2 = vectors[i + 1];
                double deltaZ = v2.z - v1.z;
                if (previousDeltaZ > 0 && deltaZ <= 0)
                {
                    if (deltaZ == 0)
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

                if (previousDeltaZ == 0 && deltaZ < 0 && plateStart != null)
                {
                    var extreme = new Vector3d(
                            v1.x,
                            v1.y + plateStart.Value / smoothingPoint,
                            GetOrdinat(v1.yz, v2.yz, v1.y + plateStart.Value / smoothingPoint));

                    extremeList.Add(extreme);

                    plateStart = null;
                }
                previousDeltaZ = deltaZ;
            }
            return new SmoothPoints() { Points = extremeList.ToArray() };
        }

        /// <summary>
        /// генерация данных для кэша
        /// </summary>
        private MeshData UpdateMeshData(DMesh3 mesh)
        {
            var result = new MeshData();

            var disntctXs = new List<double>();

            var veticles = mesh.Vertices();

            result.DistinctXs = veticles.Select(v => v.x).Order().Distinct().ToArray();
            result.DistinctYs = veticles.Select(v => v.y).Order().Distinct().ToArray();

            return result;
        }

        /// <summary>
        /// Дефолтные три точки
        /// </summary>
        private static Index3i Index3i => new Index3i(0, 1, 2);
    }
}
