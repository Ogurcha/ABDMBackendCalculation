using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Models.DataTransfer
{
    public class PassTypeCalculationResult
    {
        /// <summary>
        /// идентификатор искусственного сооружения
        /// </summary>
        public long IssoId { get; set; }

        /// <summary>
        /// Номер чекпоинта данного сооружения
        /// </summary>
        public int CPNumber { get; set; }

        /// <summary>
        /// идентификатор нагрузки на сооружение
        /// </summary>
        public long LoadId { get; set; }

        /// <summary>
        /// Направление физичесrого воздействия
        /// </summary>
        public DriveDirectionEnum Direction { get; set; } = DriveDirectionEnum.Bidirection;

        /// <summary>
        /// номер выбранного снипа, по которому пойдут расчет
        /// </summary>
        public SnipEnum Snip { get; set; } = SnipEnum.odm16;

        /// <summary>
        /// Рассчитанное условие пропуска
        /// </summary>
        public PassTypeEnum PassType { get; set; }

        /// <summary>
        /// можно ли проезжать (рассчитывается из PassType). 2 - зеленый свет, 0 - нельзя, 
        /// </summary>
        public AllowedEnum Allowed { get; set; }

        /// <summary>
        /// Интервалы между нагрузками.
        /// </summary>
        public required double[] Intervals { get; set; }

        /// <summary>
        /// Нагрузка тележек. не обязательна
        /// </summary>
        public string? Data { get; set; }

        public bool IsValidResponse => IssoId > 0 && CPNumber > 0;

        public Vector3I[]? TrianglesToCache { get; set; }
    }
}
