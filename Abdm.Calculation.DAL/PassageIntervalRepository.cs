using System.Data;
using Abdm.Calculation.DAL.Entities;
using Abdm.Calculation.Infrastructure.Settings;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Abdm.Calculation.DAL
{
    public class PassageIntervalRepository(IOptions<ConnectionStrings> connectionStrings) : IPassageIntervalRepository
    {
        public async Task<PassageInterval[]> GetPassageIntervals(long issoId)
        {
            using (var connection = new SqlConnection(connectionStrings.Value.MainConnection))
            {
                var paramTableName = new SqlParameter("@issoId", SqlDbType.BigInt) { Value = issoId };
                var paramId = new SqlParameter("@nPs", SqlDbType.Int) { Value = 1 };

                const string sqlQuery = @"
                SELECT b_gab, b_lp, b_pb
                FROM i_mp_proezd 
                WHERE i_mp_proezd.c_isso = @issoId 
                AND i_mp_proezd.n_ps = @nPs
                ORDER BY i_mp_proezd.n_ps, i_mp_proezd.w_proezd";

                var query = await connection.QueryAsync<PassageInterval>(
                    sqlQuery,
                    new DynamicParameters[]
                    {
                        new DynamicParameters(paramTableName),
                        new DynamicParameters(paramId)
                    },
                    commandType: CommandType.Text);

                return query.ToArray();
            }
        }
    }
}
