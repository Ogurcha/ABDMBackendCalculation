using System.Data;
using Abdm.Calculation.Infrastructure.Settings;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Abdm.Calculation.DAL
{
    /// <summary>
    /// Репозиторий для работы с поверхностью влияния
    /// </summary>
    public class SurfaceRepository(IOptions<ConnectionStrings> connectionStrings) : ISurfaceRepository
    {
        /// <summary>
        /// Получает массив байтов из бд, содержащих информацию о поверхности влияния
        /// </summary>
        public async Task<byte[]?> GetSurfaceData(long issoId, int checkpointNumber)
        {
            using (var connection = new NpgsqlConnection(connectionStrings.Value.MainConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@issoId", issoId, DbType.Int64);
                parameters.Add("@cpNumber", checkpointNumber, DbType.Int32);

                const string sqlQuery = @"
                SELECT data
                FROM i_checkpoint 
                WHERE c_isso = @issoId 
                AND n = @cpNumber";

                var query = await connection.QueryAsync<byte[]>(
                    sqlQuery,
                    parameters,
                    commandType: CommandType.Text);

                return query.FirstOrDefault();
            }
        }
    }
}
