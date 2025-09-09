using System.ComponentModel;

namespace Abdm.Calculation.BLL.Enums
{
    /// <summary>
    /// Условия пропуска
    /// </summary>
    public enum PassTypeEnum
    {
        [Description("Нет сведений")]
        Unknown,
        [Description("Пропуск возможен без ограничений")]
        NoLimit,
        [Description("Пропуск возможен при отсутствии пешеходов на тротуарах")]
        WithoutPedestian,
        [Description("Пропуск возможен с ограничением скорости до 10км/ч")]
        MaxSpeed10,
        [Description("Пропуск возможен в одиночном порядке с ограничением скорости до 10км/ч")]
        SingleAutoOnly,
        [Description("Пропуск возможен в одиночном порядке, с ограничением скорости до 10км/ч и положения")]
        SingleOnlyAndPlace,
        [Description("Пропуск невозможен")]
        Denied
    }
}
