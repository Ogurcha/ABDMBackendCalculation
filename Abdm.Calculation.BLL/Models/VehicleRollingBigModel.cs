using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Models
{
    public class VehicleRollingBigModel
    {
        public required VehicleRollingSmallModel Data { get; set; }

        public required PassageInterval[] Intervals { get; set; }

        public required RoadRule[] RoadRules { get; set; }

        public required Mesh Mesh { get; set; }

        public Mesh? SecondaryMesh { get; set; }

        public void FlipMeshes()
        {
            if (SecondaryMesh != null)
            {
                (SecondaryMesh, Mesh) = (Mesh, SecondaryMesh);
            }
        }
    }
}
