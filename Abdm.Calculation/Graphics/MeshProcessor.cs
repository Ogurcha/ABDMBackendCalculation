using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using g4;

namespace Abdm.Calculation.G4
{
    public class MeshProcessor : IMeshProcessor
    {
        /// <summary>
        /// возврает меш по массиву точек
        /// </summary>
        public DMeshAABBTree3 GetMeshFromPoints(Vector3[] points)
        {
            ArgumentNullException.ThrowIfNull(points);
            if (points.Length % 3 != 0)
                throw new Exception("input points are not valid");

            var mesh = DMesh3Builder.Build<Vector3d, Index3i, Vector3d>(
                points.Select(p => new Vector3d(p.X, p.Y, p.Z)), GetTriangles123(points: points));

            var meshAABBTree = new DMeshAABBTree3(mesh, true);

            return meshAABBTree;
        }

        /// <summary>
        /// Возвращает результат пересечения поверхности с плоскостью, параллельной плоскости YZ
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="X"></param>
        public void MakeProfileYZ(DMeshAABBTree3 mesh, double X)
        {
            var planeNormal = new Vector3d(1, 0, 0); 
            var planePoint = new Vector3d(X, 0, 0); 
            var plane = new Plane3d(planeNormal, planePoint);

            plane
            new DMeshAABBTree3();
            var intersections = mesh.FindAllIntersections();

            intersections.Segments

            //var planeMesh = new DMeshAABBTree3(plane);
            //MeshQueries.TrianglesIntersection(mesh, plane)
            //Ray3d ray = new Ray3d(origin, direction);
            // if (hit_tid != DMesh3.InvalidID) {
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
