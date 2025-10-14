using System.Data;
using Abdm.Calculation.DAL.Entities;
using Abdm.Calculation.Infrastructure.Settings;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Abdm.Calculation.DAL
{
    public class PassageIntervalRepository(IOptions<ConnectionStrings> connectionStrings) : IPassageIntervalRepository
    {
        public async Task<PassageIntervalDto[]> GetPassageIntervals(long issoId, CancellationToken cancellationToken)
        {
            using (var connection = new NpgsqlConnection(connectionStrings.Value.MainConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@issoId", issoId, DbType.Int64);
                parameters.Add("@nPs", 1, DbType.Int32);

                const string sqlQuery = @"
                SELECT b_gab, b_ogr_l, b_ogr_r, b_lp, b_pb, k_polos, i_mp_proezd.w_proezd
                FROM i_mp_proezd 
                JOIN i_proezd
                ON i_proezd.w_proezd = i_mp_proezd.w_proezd
                WHERE i_mp_proezd.c_isso = @issoId 
                AND i_mp_proezd.n_ps = @nPs
                AND i_proezd.c_isso = @issoId
                ORDER BY i_mp_proezd.n_ps, i_mp_proezd.w_proezd";

                var command = new CommandDefinition(
                    sqlQuery,
                    parameters,
                    commandType: CommandType.Text,
                    cancellationToken: cancellationToken
                    );

                var query = await connection.QueryAsync<PassageIntervalDto>(
                    command);

                return query.ToArray();
            }
        }
    }
}
