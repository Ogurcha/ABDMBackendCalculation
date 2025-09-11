using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.DAL;
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Services
{
    public class SurfaceDataService(ISurfaceRepository repository) : ISurfaceDataService
    {
        public async Task<SurfaceData?> GetSurfaceData(long issoId, int checkpointNumber)
        {
            var data = await repository.GetSurfaceData(issoId, checkpointNumber);
            using MemoryStream stream = new MemoryStream();
            using BinaryReader reader = new BinaryReader(stream);

            var isSymmetric = reader.ReadBoolean();
            var isGridRegular = reader.ReadBoolean();
            var pointsCount = reader.ReadInt32();
            var pointsList = GetPointsList(pointsCount, reader).ToArray();
            var trianglesCount = reader.ReadInt32();
            (int, int, int)[]? trianglesList = trianglesCount > 0
                ? GetTrianglesList(trianglesCount, reader).ToArray()
                : null;

            return new SurfaceData
            {
                IsSymmetric = isSymmetric,
                IsGridRegular = isGridRegular,
                PointsList = pointsList,
                TriangleList = trianglesList,
                TrianglesCount = trianglesCount,
                PointsCount = pointsCount
            };

            static IEnumerable<(double, double, double)> GetPointsList(int pointsCount, BinaryReader reader)
            {
                for (int i = 0; i < pointsCount; i++)
                {
                    yield return new(reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble());
                }
            }
            static IEnumerable<(int, int, int)> GetTrianglesList(int trianglesCount, BinaryReader reader)
            {
                for (int i = 0; i < trianglesCount; i++)
                {
                    yield return new(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
                }
            }
        }
    }
}
