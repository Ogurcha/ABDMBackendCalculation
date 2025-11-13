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

        #region PillarProperties

        /// <summary>
        /// Отдельная устойчивость при расчете опор
        /// </summary>
        [JsonPropertyName("SuperStrength")]
        public double? SuperStrength { get; set; }
        #endregion

        #region SteelConcreteProperties

        [JsonPropertyName("workSign")]
        public double? WorkSign { get; set; }

        [JsonPropertyName("Es")]
        public double? Es { get; set; }

        [JsonPropertyName("Ea")]
        public double? Ea { get; set; }

        [JsonPropertyName("Eb")]
        public double? Eb { get; set; }

        [JsonPropertyName("ϕ_kr")]
        public double? TetaKr { get; set; }

        [JsonPropertyName("ε_b_lim")]
        public double? EpsilonBetaLim { get; set; }

        [JsonPropertyName("Rs1")]
        public double? Rs1 { get; set; }

        [JsonPropertyName("Rs2")]
        public double? Rs2 { get; set; }

        [JsonPropertyName("Rr")]
        public double? Rr { get; set; }

        [JsonPropertyName("Rb")]
        public double? Rb { get; set; }

        [JsonPropertyName("tmax")]
        public double? Tmax { get; set; }

        [JsonPropertyName("plateType")]
        public double? PlateType { get; set; }

        [JsonPropertyName("L")]
        public double? L { get; set; }

        [JsonPropertyName("Sd")]
        public double? Sd { get; set; }

        [JsonPropertyName("σ_b_kr")]
        public double? SigmaBetaKr { get; set; }

        [JsonPropertyName("σ_a_kr")]
        public double? SigmaAlfaKr { get; set; }

        [JsonPropertyName("σ_b_shr")]
        public double? SigmaBetaShr { get; set; }

        [JsonPropertyName("σ_a_shr")]
        public double? SigmaAlfaShr { get; set; }

        [JsonPropertyName("σ_b_t")]
        public double? SigmaBetaT { get; set; }

        [JsonPropertyName("σ_a_t")]
        public double? SigmaAlfaT { get; set; }

        [JsonPropertyName("M1")]
        public double? M1 { get; set; }

        [JsonPropertyName("M2g")]
        public double? M2g { get; set; }

        [JsonPropertyName("χ1")]
        public double? Xsi1 { get; set; }

        [JsonPropertyName("Mp")]
        public double? Mp { get; set; }
        #endregion
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
