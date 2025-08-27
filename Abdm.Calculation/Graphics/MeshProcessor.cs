using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Abdm.Calculation.Graphics;
using g4;
using gs;

namespace Abdm.Calculation.G4
{
    public class MeshProcessor : IMeshProcessor
    {
        /// <summary>
        /// возврает меш по массиву точек
        /// </summary>
        public Mesh GetMeshFromPoints(Vector3[] points)
        {
            ArgumentNullException.ThrowIfNull(points);
            if (points.Length % 3 != 0)
                throw new Exception("input points are not valid");

            var mesh = DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(
                points.Select(p => new Vector3d(p.X, p.Y, p.Z)), GetTriangles123(points: points));

            var data = GetMeshData(mesh, []);

            var meshAABBTree = new DMeshAABBTree3(mesh, true);

            return new Mesh { Tree = meshAABBTree, Data = data };
        }

        /// <summary>
        /// генерация данных для кэша
        /// </summary>
        public MeshData GetMeshData(DMesh3 mesh, double[] Wheels)
        {
            var result = new MeshData();

            var disntctXs = new List<double>();
            
            var veticles = mesh.Vertices().OrderBy(v => v.x).ToList();
            var first = veticles.First();
            result.MinX = first.x; result.MinY = first.y; result.MinZ = first.z;
            result.MaxX = first.x; result.MaxY = first.y; result.MaxZ = first.z;

            double distinctXsChecker = result.MinX - 1;
            foreach (var v in veticles)
            {
                if (result.MinY > v.y) result.MinY = v.y;
                if (result.MinZ > v.z) result.MinZ = v.z;
                if (result.MaxY < v.y) result.MaxY = v.y;
                if (result.MaxZ < v.z) result.MaxZ = v.z;
                
                if (distinctXsChecker != v.x)
                {
                    distinctXsChecker = v.x;
                    disntctXs.Add(v.x);
                }
            }   
            result.DistinctXs = disntctXs.ToArray();
            result.DistinctYs = veticles.Select(v => v.y).Order().Distinct().ToArray(); 

            return result;
        }

        /// <summary>
        /// Возвращает результат пересечения поверхности с плоскостью, параллельной плоскости YZ
        /// </summary>
        public IEnumerable<Vector3d> MakeProfileYZ(Mesh mesh, double X)
        {
            var planeYZMesh = DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(
                    [   
                        new Vector3d(X, double.MinValue, double.MinValue),
                        new Vector3d(X, double.MaxValue, double.MinValue),
                        new Vector3d(X, double.MinValue, double.MaxValue)
                    ],
                    [
                        new Index3i(1, 2, 3)
                    ], []
                );
            var planeYZ = new DMeshAABBTree3(planeYZMesh, true);

            var intersection = mesh.Tree.FindAllIntersections( planeYZ );

            return intersection.Points.Select(p => p.point).OrderBy(p => p.y);
        }

        /// <summary>
        /// Возвращает результат пересечения поверхности с плоскостью, параллельной плоскости YZ
        /// Пересечение с плоскостью YZ - это равносильно пересечению с пучком лучей
        /// Плоскость это (Xconst, ?, ?)
        /// Лучи это (Xconst, Y[] - пучок с различными Yами, Z: -беск < ? < +беск)
        /// </summary>
        public IEnumerable<Vector3d> MakeProfileYZ_VIP(Mesh mesh, double X)
        {
            var rays = new List<Ray3d>();
            var minZ = mesh.Data.MinZ;
            var minMinZ = minZ - 1d;
            foreach (var y in mesh.Data.DistinctYs)
            {
                var origin = new Vector3d(X, y, minMinZ);
                var direction = new Vector3d(X, y, minZ);
                rays.Add(new Ray3d(origin, direction));
            }

            foreach (var ray in rays)
            {
                int hit_tid = mesh.Tree.FindNearestHitTriangle(ray);
                if (hit_tid != DMesh3.InvalidID)
                {
                    IntrRay3Triangle3 intr = MeshQueries.TriangleIntersection(mesh.Tree.Mesh, hit_tid, ray);
                    yield return intr.TriangleBaryCoords;
                }
            }
        }

        /// <summary>
        /// Получить коллекциу полигонов, подразумевая, что массив точек сгруппирован по 3, то есть точки 1,2,3 - это первый полигон, 4,5,6 - второй и т.д.
        /// </summary>
        private IEnumerable<Index3i> GetTriangles123(Vector3[] points)
        {
            for (int i = 0; i < points.Length / 3; i++)
            {
                var triangle = new Index3i(i, i + 1, i + 2);

                yield return triangle;
            }
        }

    }
}
