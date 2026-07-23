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
    public class PillarMaterialRepository(IOptions<ConnectionStrings> connectionStrings) : IPillarMaterialRepository
    {
        /// <summary>
        /// Возвращает типы материалов поверхности
        /// </summary>
        public async Task<PillarMaterialDto?> GetPillarMaterial(long issoId, int substructureId, CancellationToken cancellationToken)
        {
            using (var connection = new NpgsqlConnection(connectionStrings.Value.MainConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@issoId", issoId, DbType.Int64);
                parameters.Add("@sId", substructureId, DbType.Int32);

                const string sqlQuery = @"
                SELECT c_matop, c_typop 
                FROM i_opora 
                WHERE c_isso = @issoId 
                AND n = @sId";

                var command = new CommandDefinition(
                    sqlQuery,
                    parameters: parameters,
                    commandType: CommandType.Text,
                    cancellationToken: cancellationToken
                    );

                var query = await connection.QueryAsync<PillarMaterialDto>(command);

                return query.FirstOrDefault();
            }
        }
    }
}