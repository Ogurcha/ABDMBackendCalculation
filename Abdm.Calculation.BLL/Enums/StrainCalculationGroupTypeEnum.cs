using System.ComponentModel;

namespace Abdm.Calculation.BLL.Enums
{
    public enum StrainCalculationGroupTypeEnum
    {
        Unknown = -1,

        /// <summary>
        /// Обычный расчет
        /// </summary>
        [Description("Обычный расчет")]
        Default = 0,

        /// <summary>
        /// По сопоставлению воздействий
        /// </summary>
        [Description("По сопоставлению воздействий")]
        Pillar = 1,

        /// <summary>
        /// Сталежелезобетонный элемент. Прочность при плоском изгибе
        /// </summary>
        [Description("Сталежелезобетонный элемент. Прочность при плоском изгибе")]
        SteelConcrete = 2,
    }
}
