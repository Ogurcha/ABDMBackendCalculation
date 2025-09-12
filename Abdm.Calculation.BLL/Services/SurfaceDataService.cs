using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.DAL;
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Services
{
    public class SurfaceDataService(ISurfaceRepository repository) : ISurfaceDataService
    {
        private const int UsefulDataStartingPosition = 16;

        public async Task<SurfaceData?> GetSurfaceData(long issoId, int checkpointNumber)
        {
            var data = await repository.GetSurfaceData(issoId, checkpointNumber);
            if (data == null || data.Length <= UsefulDataStartingPosition)
            {
                return null;
            }
            using MemoryStream stream = new MemoryStream(data);
            stream.Position += UsefulDataStartingPosition;
            using BinaryReader reader = new BinaryReader(stream);

            var isSymmetric = reader.ReadBoolean();
            var isGridRegular = reader.ReadBoolean();
            var pointsCount = reader.ReadInt32();
            var pointsList = GetPointsList(pointsCount, reader).ToArray();
            var trianglesCount = reader.ReadInt32();
            (int, int, int)[]? trianglesList = trianglesCount > 0
                ? GetTrianglesList(pointsCount, trianglesCount, reader).ToArray()
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
            static IEnumerable<(int, int, int)> GetTrianglesList(int pointsCount, int trianglesCount, BinaryReader reader)
            {
                for (int i = 0; i < trianglesCount; i++)
                {
                    var p1 = reader.ReadInt32();
                    var p2 = reader.ReadInt32();
                    var p3 = reader.ReadInt32();
                    if (p1 >= 0 && p2 >= 0 && p3 >= 0 && p1 < pointsCount && p2 < pointsCount && p3 < pointsCount)
                    {
                        yield return new(p1, p2, p3);
                    }
                }
            }
        }
    }
}
