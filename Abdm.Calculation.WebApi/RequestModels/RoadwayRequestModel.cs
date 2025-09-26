using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.RequestModels
{
    public class RoadwayRequestModel
    {
        /// <summary>
        /// Количество путей у моста
        /// </summary>
        [JsonPropertyName("line_number")]
        public int LineNumber { get; set; }

        /// <summary>
        /// Максимальное возвышение профиля моста
        /// </summary>
        [JsonPropertyName("road_height")]
        public double RoadHeight { get; set; }

        /// <summary>
        /// Отступ слева
        /// </summary>
        [JsonPropertyName("left_safeline")]
        public double LeftSafeline { get; set; }

        /// <summary>
        /// Отступ справа
        /// </summary>
        [JsonPropertyName("right_safeline")]
        public double RightSafeline { get; set; }

        /// <summary>
        /// Сдвиг позиции по X координате. Если точки начинаются не в нуле по иксу, то сдвиг нужен для компенсации
        /// </summary>
        [JsonPropertyName("position_shift")]
        public double PositionShift { get; set; }
    }
}
