namespace Abdm.Calculation.WebApi.RequestModels
{
    public class RoadwayRequestModel
    {
        /// <summary>
        /// Количество путей у моста
        /// </summary>
        public int line_number { get; set; }

        /// <summary>
        /// Максимальное возвышение профиля моста
        /// </summary>
        public double road_height { get; set; }

        /// <summary>
        /// Отступ слева
        /// </summary>
        public double left_safeline { get; set; }

        /// <summary>
        /// Отступ справа
        /// </summary>
        public double right_safeline { get; set; }

        /// <summary>
        /// Сдвиг позиции по X координате. Если точки начинаются не в нуле по иксу, то сдвиг нужен для компенсации
        /// </summary>
        public double position_shift { get; set; }
    }
}
