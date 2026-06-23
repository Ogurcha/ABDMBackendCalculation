using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    /// <summary>
    /// Input parameters for steel-concrete composite section calculations.
    /// All lengths are in meters, areas in m², first moments in m³ and second moments in m⁴ unless otherwise noted.
    /// Property comments include the original Russian parameter names for traceability.
    /// </summary>
    public class SteelConcreteInputParametersModel
    {
        /// <summary>
        /// Расчетная ширина плиты, м
        /// Design (effective) slab width, m.
        /// </summary>
        [JsonPropertyName("designSlabWidth")]
        public decimal DesignSlabWidth { get; set; }

        /// <summary>
        /// Расчетная толщина плиты, м
        /// Design slab thickness, m.
        /// </summary>
        [JsonPropertyName("designSlabThickness")]
        public decimal DesignSlabThickness { get; set; }

        /// <summary>
        /// Зазор между верхним поясом и низом расчетной плиты, м
        /// Clearance / gap between the top flange and the bottom of the slab, m.
        /// </summary>
        [JsonPropertyName("gapTopFlangeToSlabBottom")]
        public decimal GapTopFlangeToSlabBottom { get; set; }

        /// <summary>
        /// Листы верхнего пояса ВП, толщина × ширина, м
        /// Top flange plate thickness (m).
        /// </summary>
        [JsonPropertyName("topFlangePlateThickness")]
        public decimal TopFlangePlateThickness { get; set; }

        /// <summary>
        /// Листы верхнего пояса ВП, толщина × ширина, м
        /// Top flange plate width (m).
        /// </summary>
        [JsonPropertyName("topFlangePlateWidth")]
        public decimal TopFlangePlateWidth { get; set; }

        /// <summary>
        /// Лист вертикальной стенки ВЛ, толщина × ширина, м
        /// Web (vertical plate) thickness, m.
        /// </summary>
        [JsonPropertyName("WebPlateThickness")]
        public decimal WebPlateThickness { get; set; }

        /// <summary>
        /// Лист вертикальной стенки ВЛ, толщина × ширина, м
        /// Web (vertical plate) width, m.
        /// </summary>
        [JsonPropertyName("WebPlateWidth")]
        public decimal WebPlateWidth { get; set; }

        /// <summary>
        /// Листы нижнего пояса НП, толщина × ширина, м
        /// Bottom flange plate thickness, m.
        /// </summary>
        [JsonPropertyName("BottomFlangePlateThickness")]
        public decimal BottomFlangePlateThickness { get; set; }

        /// <summary>
        /// Листы нижнего пояса НП, толщина × ширина, м
        /// Bottom flange plate width, m.
        /// </summary>
        [JsonPropertyName("BottomFlangePlateWidth")]
        public decimal BottomFlangePlateWidth { get; set; }

        /// <summary>
        /// Высота стальной балки, м
        /// Steel beam overall height, m.
        /// </summary>
        [JsonPropertyName("steelBeamHeight")]
        public decimal SteelBeamHeight { get; set; }

        /// <summary>
        /// Высота расчетного сечения объединенной балки, м
        /// Height of the design (composite) section, m.
        /// </summary>
        [JsonPropertyName("compositeSectionHeight")]
        public decimal CompositeSectionHeight { get; set; }

        /// <summary>
        /// Площадь продольной арматуры, учитываемой в расчете, м2
        /// Longitudinal reinforcement (reinforcing steel) area considered in the calculation, m².
        /// </summary>
        [JsonPropertyName("longitudinalReinforcementArea")]
        public decimal LongitudinalReinforcementArea { get; set; }

        /// <summary>
        /// Коэффициент, учитывающий развитие ограниченных пластических деформаций
        /// Coefficient accounting for the development of limited plastic strains (dimensionless).
        /// </summary>
        [JsonPropertyName("limitedPlasticDeformationFactor")]
        public decimal LimitedPlasticDeformationFactor { get; set; }

        /// <summary>
        /// Площадь сечения стальной балки, м
        /// Steel section area, m².
        /// </summary>
        [JsonPropertyName("steelSectionArea")]
        public decimal SteelSectionArea { get; set; }

        /// <summary>
        /// Статический момент сечения стальной балки, м3
        /// First moment (static moment) of the steel section, m³.
        /// </summary>
        [JsonPropertyName("staticMomentSteelSection")]
        public decimal StaticMomentSteelSection { get; set; }

        /// <summary>
        /// Площадь сечения стальной балки (включая арматуру), м
        /// Steel section area including reinforcement, m².
        /// </summary>
        [JsonPropertyName("steelSectionAreaWithReinforcement")]
        public decimal SteelSectionAreaWithReinforcement { get; set; }

        /// <summary>
        /// Статический момент сечения стальной балки с арматурой, м3
        /// First moment of area of the steel section including reinforcement, m³.
        /// </summary>
        [JsonPropertyName("staticMomentSteelSectionWithReinforcement")]
        public decimal StaticMomentSteelSectionWithReinforcement { get; set; }

        /// <summary>
        /// Момент инерции сечения стальной балки, м4
        /// Moment of inertia (second moment of area) of the steel section, m⁴.
        /// </summary>
        [JsonPropertyName("momentOfInertiaSteelSection")]
        public decimal MomentOfInertiaSteelSection { get; set; }

        /// <summary>
        /// Момент инерции сечения стальной балки относительно собственного центра тяжести, м4
        /// Moment of inertia of the steel section about its own centroid, m⁴.
        /// </summary>
        [JsonPropertyName("momentOfInertiaSteelSectionAboutCentroid")]
        public decimal MomentOfInertiaSteelSectionAboutCentroid { get; set; }

        /// <summary>
        /// Положение центра тяжести стального сечения Cs, м
        /// Position of the centroid of the steel section (Cs), m.
        /// </summary>
        [JsonPropertyName("steelSectionCentroidPosition")]
        public decimal SteelSectionCentroidPosition { get; set; }

        /// <summary>
        /// Расстояние от центра тяжести стального сечения Cs до верхней фибры стальной балки, м
        /// Distance from the steel section centroid (Cs) to the top fiber of the steel beam, m.
        /// </summary>
        [JsonPropertyName("distanceFromSteelCentroidToTopFiber")]
        public decimal DistanceFromSteelCentroidToTopFiber { get; set; }

        /// <summary>
        /// Момент инерции стальной части сечения, м4
        /// Moment of inertia of the steel part of the section, m⁴.
        /// </summary>
        [JsonPropertyName("momentOfInertiaSteelPart")]
        public decimal MomentOfInertiaSteelPart { get; set; }

        /// <summary>
        /// Момент инерции стального сечения (включая арматуру), м4
        /// Moment of inertia of the steel section including reinforcement, m⁴.
        /// </summary>
        [JsonPropertyName("momentOfInertiaSteelSectionWithReinforcement")]
        public decimal MomentOfInertiaSteelSectionWithReinforcement { get; set; }

        /// <summary>
        /// Положение центра тяжести стального сечения (включая арматуру), м
        /// Position of the centroid of the steel section including reinforcement, m.
        /// </summary>
        [JsonPropertyName("steelSectionCentroidWithReinforcementPosition")]
        public decimal SteelSectionCentroidWithReinforcementPosition { get; set; }

        /// <summary>
        /// Момент сопротивления верхнего пояса стальной части, м3
        /// Section modulus (resistance moment) of the top flange of the steel part, m³.
        /// </summary>
        [JsonPropertyName("sectionModulusTopFlangeSteelPart")]
        public decimal SectionModulusTopFlangeSteelPart { get; set; }

        /// <summary>
        /// Момент сопротивления нижнего пояса стальной части, м3
        /// Section modulus (resistance moment) of the bottom flange of the steel part, m³.
        /// </summary>
        [JsonPropertyName("sectionModulusBottomFlangeSteelPart")]
        public decimal SectionModulusBottomFlangeSteelPart { get; set; }
    }
}
