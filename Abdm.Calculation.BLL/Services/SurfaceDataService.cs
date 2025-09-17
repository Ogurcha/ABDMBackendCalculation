using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.DAL;
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Services
{
    public class SurfaceDataService(ISurfaceRepository repository) : ISurfaceDataService
    {
        /// <summary>
        /// Расшифровывает байт массив и получает информацию о поверхности влияния
        /// </summary>
        public async Task<SurfaceData?> GetSurfaceData(long issoId, int checkpointNumber)
        {
            var data = await repository.GetSurfaceData(issoId, checkpointNumber);
            using MemoryStream stream = new MemoryStream();
            using BinaryReader reader = new BinaryReader(stream);

            var isSymmetric = reader.ReadBoolean();
            var isGridRegular = reader.ReadBoolean();
            var pointsCount = reader.ReadInt32();
            var points = GetPoints(pointsCount, reader).ToArray();
            var trianglesCount = reader.ReadInt32();
            (int, int, int)[]? triangles = trianglesCount > 0
                ? GetTriangles(pointsCount, trianglesCount, reader).ToArray()
                : null;

            return new SurfaceData
            {
                IsSymmetric = isSymmetric,
                IsGridRegular = isGridRegular,
                Points = points,
                Triangles = triangles,
                TrianglesCount = trianglesCount,
                PointsCount = pointsCount
            };

            static IEnumerable<(double X, double Y, double Z)> GetPoints(int pointsCount, BinaryReader reader)
            {
                for (int i = 0; i < pointsCount; i++)
                {
                    yield return new (reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble());
                }
            }

            static IEnumerable<(int, int, int)> GetTriangles(int pointsCount, int trianglesCount, BinaryReader reader)
            {
                for (int i = 0; i < trianglesCount; i++)
                {
                    var p1 = reader.ReadInt32();
                    var p2 = reader.ReadInt32();
                    var p3 = reader.ReadInt32();
                    if (p1 >= (int)default && p2 >= (int)default && p3 >= (int)default 
                        && p1 < pointsCount && p2 < pointsCount && p3 < pointsCount)
                    {
                        yield return new(p1, p2, p3);
                    }
                }
            }
        }
    }
}
