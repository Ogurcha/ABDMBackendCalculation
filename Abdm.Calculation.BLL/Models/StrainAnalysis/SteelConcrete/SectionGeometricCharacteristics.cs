namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class SectionGeometricCharacteristics
    {
        /// <summary>
        /// Приведенная к стали площадь сечения плиты без учета арматуры, м2
        /// Slab area reduced to steel, excluding reinforcement, m².
        /// </summary>
        public decimal SlabAreaReducedToSteelExcludingReinforcement { get; set; }

        /// <summary>
        /// Приведенная к стали площадь сечения плиты с учетом арматуры, м2
        /// Slab area reduced to steel including reinforcement, m².
        /// </summary>
        public decimal SlabAreaReducedToSteelWithReinforcement { get; set; }

        /// <summary>
        /// Приведенный к стали статический момент сечения плиты, включая статический момент арматуры, м3
        /// Static (first) moment of the slab section reduced to steel including reinforcement, m³.
        /// </summary>
        public decimal SlabStaticMomentReducedToSteelWithReinforcement { get; set; }

        /// <summary>
        /// Приведенный к стали момент инерции сечения плиты, включая момент инерции арматуры, м4
        /// Moment of inertia of the slab section reduced to steel including reinforcement, m⁴.
        /// </summary>
        public decimal SlabMomentOfInertiaReducedToSteelWithReinforcement { get; set; }

        /// <summary>
        /// Приведенный к стали момент инерции сечения плиты относительно собственного центра тяжести, исключая собственный момент инерции арматуры, м4
        /// Moment of inertia of the slab section reduced to steel about its own centroid, excluding rebar self-inertia, m⁴.
        /// </summary>
        public decimal SlabMomentOfInertiaReducedToSteelAboutCentroidExcludingRebar { get; set; }

        /// <summary>
        /// Площадь сечения сталежелезобетонной балки, м2
        /// Area of the steel–reinforced concrete (composite) beam section, m².
        /// </summary>
        public decimal CompositeBeamSectionArea { get; set; }

        /// <summary>
        /// Статический момент сечения сталежелезобетонной балки, м3
        /// Static (first) moment of the composite beam section, m³.
        /// </summary>
        public decimal CompositeBeamStaticMoment { get; set; }

        /// <summary>
        /// Момент инерции сечения сталежелезобетонной балки, м4
        /// Moment of inertia of the composite beam section, m⁴.
        /// </summary>
        public decimal CompositeBeamMomentOfInertia { get; set; }

        /// <summary>
        /// Момент инерции сечения сталежелезобетонной балки относительно собственного центра тяжести, м4
        /// Moment of inertia of the composite beam section about its own centroid, m⁴.
        /// </summary>
        public decimal CompositeBeamMomentOfInertiaAboutCentroid { get; set; }

        /// <summary>
        /// Положение центра тяжести железобетонной плиты Cbr, м
        /// Position of the centroid of the reinforced concrete slab (Cbr), m.
        /// </summary>
        public decimal ConcreteSlabCentroidPosition { get; set; }

        /// <summary>
        /// Положение центра тяжести объединенного сечения Cstb, м
        /// Position of the centroid of the combined section (Cstb), m.
        /// </summary>
        public decimal CompositeSectionCentroidPosition { get; set; }

        /// <summary>
        /// Расстояние между Cstb и Cbr, м
        /// Distance between Cstb and Cbr, m.
        /// </summary>
        public decimal DistanceBetweenCompositeAndSlabCentroids { get; set; }

        /// <summary>
        /// Расстояние между положением центра тяжести стального сечения и Cbr, м
        /// Distance between the centroid of the steel section and Cbr, m.
        /// </summary>
        public decimal DistanceBetweenSteelCentroidAndSlabCentroid { get; set; }

        /// <summary>
        /// Расстояние от центра тяжести объединенного сечения до верхней фибры железобетонной плиты, м
        /// Distance from the centroid of the combined section to the top fiber of the concrete slab, m.
        /// </summary>
        public decimal DistanceFromCompositeCentroidToTopFiberOfConcreteSlab { get; set; }

        /// <summary>
        /// Расстояние от центра тяжести железобетонной плиты до верхней фибры железобетонной плиты, м
        /// Distance from the centroid of the concrete slab to the top fiber of the concrete slab, m.
        /// </summary>
        public decimal DistanceFromConcreteSlabCentroidToTopFiber { get; set; }

        /// <summary>
        /// Момент инерции объединенного сечения, м4
        /// Moment of inertia of the combined section, m⁴.
        /// </summary>
        public decimal CombinedSectionMomentOfInertia { get; set; }

        /// <summary>
        /// Моменты сопротивления (верхний и нижний), м3
        /// Section moduli / resistance moments (top and bottom), m³.
        /// </summary>
        public decimal SectionModulusTop { get; set; }

        /// <summary>
        /// Моменты сопротивления (верхний и нижний), м3
        /// Section moduli / resistance moments (top and bottom), m³.
        /// </summary>
        public decimal SectionModulusBottom { get; set; }
    }
}
