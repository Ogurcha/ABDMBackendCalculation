namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class SteelConcreteLoads
    {
        /// <summary>
        /// Bending moments from permanent loads, first stage, MN*m.
        /// (Изгибающие моменты от постоянных нагрузок первой стадии, МН*м)
        /// </summary>
        public decimal PermanentLoadsFirstStageMoments { get; set; }

        /// <summary>
        /// Bending moments from permanent loads, second stage, MN*m.
        /// (Изгибающие моменты от постоянных нагрузок второй стадии, МН*м)
        /// </summary>
        public decimal PermanentLoadsSecondStageMoments { get; set; }

        /// <summary>
        /// Bending moments from pedestrian load, MN*m.
        /// (Изгибающие моменты от пешеходной нагрузки, МН*м)
        /// </summary>
        public decimal PedestrianLoadMoments { get; set; }
    }
}
