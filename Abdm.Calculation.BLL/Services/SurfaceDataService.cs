using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.DAL;
using Abdm.Calculation.DAL.Entities;
using Abdm.Calculation.Maths.Models;
using Mapster;

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
        public async Task<ResultExceptionContainer<SurfaceDataDto>> GetSurfaceData(long issoId, int checkpointNumber, CancellationToken cancellationToken)
        {
            var data = await repository.GetSurfaceData(issoId, checkpointNumber, cancellationToken);
            if (data?.data == null || data?.data.Length <= UsefulDataStartingPosition)
            {
                return new ResultExceptionContainer<SurfaceDataDto>(new Exception(UnsupportedBinaryTypeStr));
            }
            using MemoryStream stream = new MemoryStream(data!.data);
            using BinaryReader reader = new BinaryReader(stream);
            if (reader.ReadInt32() > OldClientFormatCondition)
            {
                return new ResultExceptionContainer<SurfaceDataDto>(new Exception(UnsupportedBinaryTypeStr));
            }
            stream.Position = UsefulDataStartingPosition;
            var surface = data.Adapt<SurfaceDataDto>();

            if (surface.StrainCalculationType != DAL.Enums.StrainCalculationTypeEnum.st70)
            {
                surface.IsSymmetric = reader.ReadBoolean();
                surface.IsGridRegular = reader.ReadBoolean();
                surface.PointsCount = reader.ReadInt32();
                surface.Points = ReadPoints3D(reader, surface.PointsCount).ToArray();
                surface.TrianglesCount = reader.ReadInt32();
                surface.Triangles = surface.TrianglesCount > 0
                    ? ReadTriangles(reader, surface.TrianglesCount, surface.PointsCount).ToArray()
                    : null;
            }
            else
            {
                surface.PointsCount = reader.ReadInt32();
                surface.Points = ReadPointsYZ(reader, surface.PointsCount).ToArray();
            }

            return new ResultExceptionContainer<SurfaceDataDto>(surface);
        }

        private IEnumerable<Vector3D> ReadPointsYZ(BinaryReader reader, int pointsToRead)
        {
            for (int i = 0; i < pointsToRead; i++)
            {
                yield return new((double)default, reader.ReadDouble(), reader.ReadDouble());
            }
        }

        private IEnumerable<Vector3D> ReadPoints3D(BinaryReader reader, int pointsToRead)
        {
            for (int i = 0; i < pointsToRead; i++)
            {
                yield return new(reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble());
            }
        }

        private IEnumerable<Vector3I> ReadTriangles(BinaryReader reader, int trianglesToRead, int pointsCount)
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
