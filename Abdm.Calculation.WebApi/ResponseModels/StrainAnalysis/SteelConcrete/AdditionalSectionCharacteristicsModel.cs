using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class AdditionalSectionCharacteristicsModel
    {
        /// <summary>
        /// Расстояние между центрами тяжести объединенного сечения и плиты, м
        /// Distance between centroids of the combined section and the slab, m.
        /// </summary>
        [JsonPropertyName("distanceCombined")]
        public decimal DistanceCombined { get; set; }

        /// <summary>
        /// Расстояние между центрами тяжести стального сечения и плиты, м
        /// Distance between centroids of the steel section and the slab, m.
        /// </summary>
        [JsonPropertyName("distanceSteel")]
        public decimal DistanceSteel { get; set; }

        /// <summary>
        /// Статический момент объединенного сечения, м3
        /// Static (first) moment of the combined section, m³.
        /// </summary>
        [JsonPropertyName("combinedSectionStaticMoment")]
        public decimal CombinedSectionStaticMoment { get; set; }

        /// <summary>
        /// Площадь вертикального листа стальной балки, м2
        /// Area of the vertical plate (web) of the steel beam, m².
        /// </summary>
        [JsonPropertyName("verticalPlateAreaSteelBeam")]
        public decimal VerticalPlateAreaSteelBeam { get; set; }

        /// <summary>
        /// Площадь горизонтального листа нижнего пояса балки, м2
        /// Area of the horizontal plate of the bottom flange of the beam, m².
        /// </summary>
        [JsonPropertyName("horizontalPlateAreaBottomFlange")]
        public decimal HorizontalPlateAreaBottomFlange { get; set; }

        /// <summary>
        /// Величина по формуле 5.3.9 ОДМ 218.4.027-2016 Zb1
        /// Value computed by formula 5.3.9 from ODM 218.4.027-2016 (Zb1).
        /// </summary>
        [JsonPropertyName("zb1")]
        public decimal Zb1 { get; set; }

        /// <summary>
        /// Величина по формуле 5.3.9 ОДМ 218.4.027-2016 S
        /// Value computed by formula 5.3.9 from ODM 218.4.027-2016 (S).
        /// </summary>
        [JsonPropertyName("s")]
        public decimal S { get; set; }
    }
}
