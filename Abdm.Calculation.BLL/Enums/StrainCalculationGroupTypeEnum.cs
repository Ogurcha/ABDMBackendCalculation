using System.ComponentModel;

namespace Abdm.Calculation.BLL.Enums
{
    /// <summary>
    /// Тип расчета напряжений. Влияет на выбор стратегии анализа и на структуру результирующей модели
    /// </summary>
    public enum StrainCalculationGroupTypeEnum
    {
        Unknown = -1,

        /// <summary>
        /// Обычный расчет (железобетон)
        /// </summary>
        [Description("Обычный расчет (железобетон)")]
        Default = 0,

        /// <summary>
        /// Расчёт опор
        /// </summary>
        [Description("Расчёт опор")]
        Pillar = 1,

        /// <summary>
        /// Сталежелезобетонный элемент. Прочность при плоском изгибе
        /// </summary>
        [Description("Сталежелезобетонный элемент. Прочность при плоском изгибе")]
        SteelConcrete = 2,

        /// <summary>
        /// Плита
        /// </summary>
        [Description("Плита")]
        Slab = 3,
    }
}
