using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.RequestModels
{
    public class SurfaceRequestModel
    {
        /// <summary>
        /// Массив точек, из которых состоит поверхность влияния
        /// </summary>
        [JsonPropertyName("surface_data")]
        public SurfaceDataItemRequestModel[]? SurfaceData { get; set; }

        /// <summary>
        /// Данные по опоре. Если чекпоинт не являтся опорой - массив пустой
        /// </summary>
        [JsonPropertyName("line_data")]
        public LineDataItemRequestModel[]? LineData { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по X
        /// </summary>
        [JsonPropertyName("maxX")]
        public double? MaxX { get; set; }

        /// <summary>
        /// Минимальное значение всех точек по X
        /// </summary>
        [JsonPropertyName("minX")]
        public double? MinX { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по Y
        /// </summary>
        [JsonPropertyName("maxY")]
        public double? MaxY { get; set; }

        /// <summary>
        /// Минимальное значение всех точек по Y
        /// </summary>
        [JsonPropertyName("minY")]
        public double? MinY { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по Z
        /// </summary>
        [JsonPropertyName("maxZ")]
        public double? MaxZ { get; set; }

        /// <summary>
        /// Перечисление, указывающее на то, как поверхность будет подвергаться нагрузке
        /// CpSubType в старом клиенте
        /// </summary>
        [JsonPropertyName("cpVid")]
        public int? CpVid { get; set; }

        /// <summary>
        /// Проектная устойчивость структуры. Без учёта собственного веса
        /// </summary>
        [JsonPropertyName("myStrength")]
        public double? MyStrength { get; set; }

        /// <summary>
        /// Фиксированная нагрузка от собственного веса
        /// </summary>
        [JsonPropertyName("constLoad")]
        public double? ConstLoad { get; set; }

        /// <summary>
        /// Нагрузка от пешеходов
        /// </summary>
        [JsonPropertyName("constPesh")]
        public double? ConstPesh { get; set; }

        /// <summary>
        /// Другая нагрузка
        /// </summary>
        [JsonPropertyName("constOther")]
        public double? ConstOther { get; set; }

        /// <summary>
        /// Коэффициент устойчивости. По дефолту всегда 1.
        /// </summary>
        [JsonPropertyName("kStrength")]
        public double? KStrength { get; set; }
    }

    public class SurfaceDataItemRequestModel
    {
        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }

        [JsonPropertyName("z")]
        public double Z { get; set; }
    }

    public class LineDataItemRequestModel
    {
        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }
    }
}
