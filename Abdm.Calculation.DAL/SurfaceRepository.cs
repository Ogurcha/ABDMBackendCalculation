using System.Data;
using Abdm.Calculation.Infrastructure.Settings;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

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
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
