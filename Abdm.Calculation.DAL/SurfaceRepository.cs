using System.Data;
using Abdm.Calculation.DAL.Entities;
using Abdm.Calculation.Infrastructure.Settings;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Abdm.Calculation.DAL
{
    public class SurfaceRepository(IOptions<ConnectionStrings> connectionStrings) : ISurfaceRepository
    {

        public async Task<SurfaceData?> GetSurfaceData(long issoId, int checkpointNumber)
        {
            using (var connection = new SqlConnection(connectionStrings.Value.MainConnection))
            {
                var paramIssoId = new SqlParameter("@issoId", SqlDbType.BigInt) { Value = issoId };
                var paramCheckpointNumber = new SqlParameter("@cpNumber", SqlDbType.Int) { Value = checkpointNumber };

                const string sqlQuery = @"
                SELECT data
                FROM i_checkpoint 
                WHERE c_isso = @issoId 
                AND n = @cpNumber";

                var query = await connection.QueryAsync<byte[]>(
                    sqlQuery,
                    new DynamicParameters[]
                    {
                        new DynamicParameters(paramIssoId),
                        new DynamicParameters(paramCheckpointNumber)
                    },
                    commandType: CommandType.Text);

                var data = query.FirstOrDefault();

                if (data != null)
                {
                    return ParseSurfaceData(data);
                }
                else
                {
                    return null;
                }
            }
        }

        private SurfaceData ParseSurfaceData(byte[] data)
        {
            using MemoryStream stream = new MemoryStream(data);
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
