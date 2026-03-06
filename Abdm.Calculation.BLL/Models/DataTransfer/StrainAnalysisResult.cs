using System.Text.Json.Serialization;
using Abdm.Calculation.BLL.Models.StrainAnalysis;

namespace Abdm.Calculation.BLL.Models.DataTransfer
{
    public class StrainAnalysisResult
    {
        /// <summary>
        /// идентификатор искусственного сооружения
        /// </summary>
        public long IssoId { get; set; }

        /// <summary>
        /// Номер чекпоинта данного сооружения
        /// </summary>
        public int CheckPointNumber { get; set; }

        /// <summary>
        /// идентификатор нагрузки на сооружение
        /// </summary>
        public long LoadId { get; set; }

        /// <summary>
        /// Направление физичесrого воздействия
        /// </summary>
        public int Direction { get; set; }

        /// <summary>
        /// номер выбранного снипа, по которому пойдут расчет
        /// </summary>
        public int SnipId { get; set; }

        /// <summary>
        /// Результат расчётов
        /// </summary>
        public required AnalysisSummary Data { get; set; }
    }
}
