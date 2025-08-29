using System.Collections.Generic;
using System.Numerics;
using Abdm.Calculation.Graphics;
using g4;

namespace Abdm.Calculation.G4
{
    public interface IMeshProcessor
    {
        Mesh GetMeshFromPoints(Vector3[] points);
        IEnumerable<Vector3d> MakeProfileYZ(Mesh mesh, double X);
    }
}