using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Abdm.Calculation.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Abdm.Calculation.DAL
{
    public class PassageIntervalRepository(MainDbContext dbContext) : IPassageIntervalRepository
    {

        /// <summary>
        /// Возвращает данные для расщета интервалов для данного иссо
        /// </summary>
        public async Task<PassageInterval[]> GetPassageIntervals(long issoId)
        {
            var result = new List<PassageInterval>();
            var paramTableName = new SqlParameter("@issoId", SqlDbType.BigInt) { Value = issoId };
            var paramId = new SqlParameter("@nPs", SqlDbType.Int) { Value = 1 };

            FormattableString sqlQuery = $@"selec b_gab, b_lp, b_pb
                              from i_mp_proezd 
                              where i_mp_proezd.c_isso={paramTableName.ParameterName} and i_mp_proezd.n_ps={paramId.ParameterName} order by i_mp_proezd.n_ps, i_mp_proezd.w_proezd";

            var query = dbContext.Database.SqlQuery<PassageInterval>(sqlQuery);

            foreach (var row in await query.ToListAsync())
            {
                row.SafeInterval = [
                    row.SafetyLineLeft > 0.25 ? row.SafetyLineLeft : 0.25 + row.SafetyLineLeft,
                    row.SafetyLineRight > 0.25 ? row.TotalWidth - row.SafetyLineRight : row.TotalWidth - row.SafetyLineRight - 0.25
                ];
                result.Add(row);
            }
            return result.ToArray();
        }
    }
}
