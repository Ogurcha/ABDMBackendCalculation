using System.ComponentModel;

namespace Abdm.Calculation.BLL.Enums
{
    /// <summary>
    /// Выбор формулы расчёта напряжений из полученной поверхности влияния
    /// </summary>
    public enum StrainCalculationFormulaEnum
    {
        /// <summary>
        /// Обычный расчет
        /// </summary>
        [Description("Обычный расчет")]
        Default,

        /// <summary>
        /// Плита
        /// </summary>
        [Description("Плита")]
        Slab,
    }
}
