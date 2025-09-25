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
        public async Task<byte[]?> GetSurfaceData(long issoId, int checkpointNumber, CancellationToken cancellationToken)
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

                var command = new CommandDefinition(
                    sqlQuery,
                    parameters: parameters,
                    commandType: CommandType.Text,
                    cancellationToken: cancellationToken
                    );

                var query = await connection.QueryAsync<byte[]>(command);

                return query.FirstOrDefault();
            }
        }
    }
}
