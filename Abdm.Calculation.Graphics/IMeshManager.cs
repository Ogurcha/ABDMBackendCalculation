using System.Numerics;
using Abdm.Calculation.Graphics.Entities;
using g4;

namespace Abdm.Calculation.Graphics
{
    public interface IMeshManager
    {
        Mesh GetMeshFromPoints(Vector3[] points);
        IEnumerable<Vector3d> MakeProfileYZ(Mesh mesh, double X);
    }
}