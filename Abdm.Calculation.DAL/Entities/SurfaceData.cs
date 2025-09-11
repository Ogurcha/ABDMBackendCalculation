namespace Abdm.Calculation.DAL.Entities
{
    public class SurfaceData
    {
        public bool IsSymmetric { get; set; }

        public bool IsGridRegular { get; set; }

        public int PointsCount { get; set; }

        public required (double, double, double)[] PointsList { get; set; }

        public int TrianglesCount { get; set; }

        public (int, int, int)[]? TriangleList { get; set; }
    }
}
