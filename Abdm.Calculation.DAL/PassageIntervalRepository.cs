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
        public async Task<PassageInterval[]> GetPassageIntervals(long issoId)
        {
            using (var connection = new NpgsqlConnection(connectionStrings.Value.MainConnection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@issoId", issoId, DbType.Int64);
                parameters.Add("@nPs", 1, DbType.Int32);

                const string sqlQuery = @"
                SELECT b_gab, b_lp, b_pb
                FROM i_mp_proezd 
                WHERE i_mp_proezd.c_isso = @issoId 
                AND i_mp_proezd.n_ps = @nPs
                ORDER BY i_mp_proezd.n_ps, i_mp_proezd.w_proezd";

                var query = await connection.QueryAsync<PassageInterval>(
                    sqlQuery,
                    parameters,
                    commandType: CommandType.Text);

                return query.ToArray();
            }
        }
    }
}
