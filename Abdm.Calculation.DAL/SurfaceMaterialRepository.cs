using System.Data;
using Abdm.Calculation.DAL.DataTransferObjects;
using Abdm.Calculation.DAL.Interfaces;
using Abdm.Calculation.Infrastructure.Settings;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Abdm.Calculation.DAL
{
    /// <summary>
    /// репозиторий типа материалов поверхности
    /// </summary>
    public class SurfaceMaterialRepository(IOptions<ConnectionStrings> connectionStrings) : ISurfaceMaterialRepository
    {
        /// <summary>
        /// Возвращает типы материалов поверхности
        /// </summary>
        public async Task<SurfaceMaterialDto?> GetSurfaceMaterial(long issoId, int checkpointNumber, CancellationToken cancellationToken)
        {
            using (var connection = new NpgsqlConnection(connectionStrings.Value.MainConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@issoId", issoId, DbType.Int64);
                parameters.Add("@cpNumber", checkpointNumber, DbType.Int32);

                const string sqlQuery = @"
                SELECT c_mpsbm, c_sistps, c_typps 
                FROM i_ps 
                WHERE c_isso = @issoId 
                AND n_ps = @cpNumber";

                var command = new CommandDefinition(
                    sqlQuery,
                    parameters: parameters,
                    commandType: CommandType.Text,
                    cancellationToken: cancellationToken
                    );

                var query = await connection.QueryAsync<SurfaceMaterialDto>(command);

                return query.FirstOrDefault();
            }
        }
    }
}