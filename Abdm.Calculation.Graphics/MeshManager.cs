using Abdm.Calculation.Graphics.Models;
using Abdm.Calculation.Maths.Models;
using g4;

namespace Abdm.Calculation.Graphics
{
    public class MeshManager (IEqualityComparer<double> doubleEqualityComparer) : IMeshManager
    {
        /// <summary>
        /// возврает меш по массиву точек
        /// </summary>
        public Mesh GetMeshFromPoints(Vector3D[] points, 
            Vector3I[]? trianglesFromCache,
            out Vector3I[]? trianglesToCache,
            bool mirrorZ = false)
        {
            ArgumentNullException.ThrowIfNull(points);

            Func<Vector3D, Vector3d> vectorConvertFunc = mirrorZ 
                ? (Vector3D v) => new Vector3d(v.X, v.Y, -v.Z) 
                : (Vector3D v) => new Vector3d(v.X, v.Y, v.Z);

            Vector3I[] triangleList;
            if (trianglesFromCache == null)
            {
                trianglesToCache = Triangulator.Triangulate(points);
                ArgumentNullException.ThrowIfNull(trianglesToCache);
                triangleList = trianglesToCache;
            }
            else
            {
                trianglesToCache = null;
                triangleList = trianglesFromCache;
            }
            
            var mesh = DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(
                points.Select(vectorConvertFunc),
                triangleList.Select(t => new Index3i(t.X, t.Y, t.Z)));

            var data = UpdateMeshData(mesh);

            var meshAABBTree = new DMeshAABBTree3(mesh, true);

            return new Mesh { Tree = meshAABBTree, Data = data };
        }

        /// <summary>
        /// Возвращает результат пересечения поверхности с плоскостью, параллельной плоскости YZ.
        /// Использует AABB-дерево меша, чтобы проверять только треугольники, пересекающие X.
        /// </summary>
        public IEnumerable<Vector3d>? GetIntersectionVectors(Mesh mesh, double X)
        {
            var tree = mesh.Tree;
            var dmesh = tree.Mesh;
            var points = new List<Vector3d>();

            Vector3d v0 = Vector3d.Zero, v1 = Vector3d.Zero, v2 = Vector3d.Zero;
            tree.DoTraversal(new DMeshAABBTree3.TreeTraversal
            {
                NextBoxF = (box, _) => BoxMayIntersectPlaneX(box, X),
                NextTriangleF = tid =>
                {
                    dmesh.GetTriVertices(tid, ref v0, ref v1, ref v2);
                    AddEdgePlaneIntersection(v0, v1, X, points);
                    AddEdgePlaneIntersection(v1, v2, X, points);
                    AddEdgePlaneIntersection(v2, v0, X, points);
                }
            });

            if (points.Count == 0)
            {
                return null;
            }

            // Triangulation diagonals also cross x=X and inject midpoints whose Y depends on X.
            // For an extruded pillar those midpoints are collinear with the true profile and must be
            // removed, otherwise left/right strips see different Y samples for the same surface.
            return RemoveCollinearInteriorPoints(
                points.DistinctBy(p => p.y, doubleEqualityComparer).OrderBy(p => p.y));
        }

        /// <summary>
        /// Drops interior samples that lie on the YZ segment between their neighbours.
        /// Keeps real corners of the profile polyline.
        /// </summary>
        private static List<Vector3d> RemoveCollinearInteriorPoints(IEnumerable<Vector3d> sortedByY)
        {
            var sorted = sortedByY as IList<Vector3d> ?? sortedByY.ToList();
            if (sorted.Count < 3)
            {
                return sorted.ToList();
            }

            var result = new List<Vector3d>(sorted.Count) { sorted[0] };
            for (int i = 1; i < sorted.Count - 1; i++)
            {
                var prev = result[^1];
                var curr = sorted[i];
                var next = sorted[i + 1];
                if (!AreCollinearInYz(prev, curr, next))
                {
                    result.Add(curr);
                }
            }
            result.Add(sorted[^1]);
            return result;
        }

        private static bool AreCollinearInYz(Vector3d a, Vector3d b, Vector3d c)
        {
            // 2D cross product (b-a) x (c-a) in YZ
            double cross = (b.y - a.y) * (c.z - a.z) - (b.z - a.z) * (c.y - a.y);
            return Math.Abs(cross) <= 1e-12 * (1.0 + Math.Abs(a.y) + Math.Abs(c.y) + Math.Abs(a.z) + Math.Abs(c.z));
        }

        /// <summary>
        /// генерация данных для кэша
        /// </summary>
        private MeshData UpdateMeshData(DMesh3 mesh)
        {
            var veticles = mesh.Vertices();

            var result = new MeshData
            {
                DistinctXs = veticles.Select(v => v.x).Order().Distinct(doubleEqualityComparer).ToArray()
            };
            
            return result;
        }

        /// <summary>
        /// AABB в дереве хранится в float — чуть расширяем допуск, чтобы не отбрасывать кандидатов.
        /// Точная проверка всё равно делается по double на рёбрах.
        /// </summary>
        private bool BoxMayIntersectPlaneX(AxisAlignedBox3f box, double X)
        {
            float minX = box.Min.x;
            float maxX = box.Max.x;
            if (doubleEqualityComparer.Equals(minX, X) || doubleEqualityComparer.Equals(maxX, X))
            {
                return true;
            }

            return minX <= X && X <= maxX;
        }

        private void AddEdgePlaneIntersection(Vector3d a, Vector3d b, double X, List<Vector3d> points)
        {
            bool aOnPlane = doubleEqualityComparer.Equals(a.x, X);
            bool bOnPlane = doubleEqualityComparer.Equals(b.x, X);

            if (aOnPlane && bOnPlane)
            {
                points.Add(new Vector3d(X, a.y, a.z));
                points.Add(new Vector3d(X, b.y, b.z));
                return;
            }

            if (aOnPlane)
            {
                points.Add(new Vector3d(X, a.y, a.z));
                return;
            }

            if (bOnPlane)
            {
                points.Add(new Vector3d(X, b.y, b.z));
                return;
            }

            if ((a.x - X) * (b.x - X) > 0)
            {
                return;
            }

            double t = (X - a.x) / (b.x - a.x);
            points.Add(new Vector3d(X, a.y + t * (b.y - a.y), a.z + t * (b.z - a.z)));
        }
    }
}
