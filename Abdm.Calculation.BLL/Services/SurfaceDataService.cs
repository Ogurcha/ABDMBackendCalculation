using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.DAL;
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Services
{
    public class SurfaceDataService(ISurfaceRepository repository) : ISurfaceDataService
    {
        /// <summary>
        /// старый "толстый клиент" сохраняет в первых 16-ти байтах служебный мусор.
        /// </summary>
        private const int UsefulDataStartingPosition = 16;
        /// <summary>
        /// Проверка списанная со старого клиента
        /// </summary>
        private const int OldClientFormatCondition = 10;

        private const string UnsupportedBinaryTypeStr = "Unsupported binary format";

        /// <summary>
        /// Расшифровывает байт массив и получает информацию о поверхности влияния
        /// </summary>
        public async Task<SurfaceData?> GetSurfaceData(long issoId, int checkpointNumber)
        {
            var data = await repository.GetSurfaceData(issoId, checkpointNumber);
            if (data == null || data.Length <= UsefulDataStartingPosition)
            {
                throw new Exception(UnsupportedBinaryTypeStr);
            }
            using MemoryStream stream = new MemoryStream(data);
            using BinaryReader reader = new BinaryReader(stream);
            if (reader.ReadInt32() > OldClientFormatCondition)
            {
                throw new Exception(UnsupportedBinaryTypeStr);
            }
            stream.Position = UsefulDataStartingPosition;

            var isSymmetric = reader.ReadBoolean();
            var isGridRegular = reader.ReadBoolean();
            var pointsCount = reader.ReadInt32();
            var points = ReadPoints(reader, pointsCount).ToArray();
            var trianglesCount = reader.ReadInt32();
            (int, int, int)[]? triangles = trianglesCount > 0
                ? ReadTriangles(reader, trianglesCount, pointsCount).ToArray()
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
        }

        private IEnumerable<(double X, double Y, double Z)> ReadPoints(BinaryReader reader, int pointsToRead)
        {
            for (int i = 0; i < pointsToRead; i++)
            {
                yield return new(reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble());
            }
        }

        private IEnumerable<(int, int, int)> ReadTriangles(BinaryReader reader, int trianglesToRead, int pointsCount)
        {
            for (int i = 0; i < trianglesToRead; i++)
            {
                var p1 = reader.ReadInt32();
                var p2 = reader.ReadInt32();
                var p3 = reader.ReadInt32();
                if (IsValidTriangle(pointsCount, p1, p2, p3))
                {
                    yield return (p1, p2, p3);
                }
            }
        }

        private bool IsValidTriangle(int pointsCount, int p1, int p2, int p3)
        {
            return p1 != p2 && p2 != p3 && p3 != p1 && p1 < pointsCount && p2 < pointsCount && p3 < pointsCount;
        }
    }
}
