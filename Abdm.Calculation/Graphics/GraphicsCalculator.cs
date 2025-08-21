using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using g4;

namespace Abdm.Calculation.G4
{
    public class GraphicsCalculator
    {




        /// <summary>
        /// возврает поверхность из 
        /// </summary>
        public DMesh3 GetMeshFromPoints(Vector3[] points)
        {
            if (points.Length % 3 != 0)
                throw new Exception("points are not valid");

            var mesh = DMesh3Builder.Build<Vector3d, Triangle3d, Vector3d>(
                points.Select(p => new Vector3d(p.X, p.Y, p.Z)), GetTriangles(points: points));

            return mesh;
        }


        private IEnumerable<Triangle3d> GetTriangles(Vector3[] points)
        {
            for (int i = 0; i < points.Length / 3; i++)
            {
                var triangle = new Triangle3d(
                    new Vector3d(points[i].X, points[i].Y, points[i].Z),
                    new Vector3d(points[i + 1].X, points[i + 1].Y, points[i + 1].Z),
                    new Vector3d(points[i + 2].X, points[i + 2].Y, points[i + 2].Z)
                    );

                yield return triangle;
            }
        }

    }
}
