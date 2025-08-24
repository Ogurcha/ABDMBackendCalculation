using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Abdm.Calculation.DAL
{
    public class PassageIntervalRepository(DbContext dbContext) : IPassageIntervalRepository
    {

        /// <summary>
        /// Возвращает данные для расщета интервалов для данного иссо
        /// </summary>
        public async Task<double[]> GetPassageIntervals(long issoId)
        {
            var result = new List<double>();
            var paramTableName = new SqlParameter("@issoId", SqlDbType.BigInt) { Value = issoId };
            var paramId = new SqlParameter("@nPs", SqlDbType.Int) { Value = 1 };

            FormattableString sqlQuery = $@"selec b_gab, b_lp, b_pb
                              from i_mp_proezd 
                              where i_mp_proezd.c_isso={paramTableName.ParameterName} and i_mp_proezd.n_ps={paramId.ParameterName} order by i_mp_proezd.n_ps, i_mp_proezd.w_proezd";

            var query = dbContext.Database.SqlQuery<(Double?, Double?, Double?)>(sqlQuery);

            foreach (var row in await query.ToListAsync())
            {
                var b_gab = row.Item1 != null ? row.Item1 : 0d;
                var b_lp = row.Item2 != null ? row.Item2 : 0d;
                var b_pb = row.Item3 != null ? row.Item3 : 0d;
                result.Add((double)(b_gab - b_lp - b_pb));
            }
            return result.ToArray();
        }
    }
}
