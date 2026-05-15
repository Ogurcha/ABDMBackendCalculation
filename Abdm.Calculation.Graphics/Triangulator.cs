using Abdm.Calculation.Maths.Models;
using TriangleNet.Geometry;

namespace Abdm.Calculation.Graphics
{
    public class Triangulator
    {
        public static Vector3I[]? Triangulate(IList<Vector3D> vectors)
        {
            Polygon polygon = new Polygon(vectors.Count);
            for (int i = 0; i < vectors.Count; i++)
            {
                Vector3D v = vectors[i];
                polygon.Add(new Vertex { X = v.X, Y = v.Y, ID = i });
            }
            var mesh = polygon.Triangulate();
            if (mesh == null || mesh.Triangles.Count == 0)
            {
                return null;
            }
            return mesh.Triangles.Select(t => new Vector3I { 
                X = t.GetVertexID(0), 
                Y = t.GetVertexID(1), 
                Z = t.GetVertexID(2) 
            }).ToArray();
        }
    }
}
