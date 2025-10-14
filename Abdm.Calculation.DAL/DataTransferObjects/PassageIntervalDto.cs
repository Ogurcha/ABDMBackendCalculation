namespace Abdm.Calculation.DAL.Entities
{
    public class PassageIntervalDto
    {
        /// <summary>
        /// Общая ширина интервала
        /// </summary>
        public double b_gab { get; set; }
        
        /// <summary>
        /// Ограждение слева
        /// </summary>
        public double? b_ogr_l { get; set; }

        /// <summary>
        /// Ограждение справа
        /// </summary>
        public double? b_ogr_r { get; set; }

        /// <summary>
        /// Полоса безопасности слева
        /// </summary>
        public double? b_lp { get; set; }

        /// <summary>
        /// Полоса безопасности справа
        /// </summary>
        public double? b_pb { get; set; }
        
        /// <summary>
        /// Количество полос движения на данном интервале
        /// </summary>
        public int k_polos { get; set; }

        /// <summary>
        /// Тип движения на интервале
        /// </summary>
        public int w_proezd { get; set; }
    }
}
