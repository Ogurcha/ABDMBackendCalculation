using System.ComponentModel;

namespace Abdm.Calculation.Models
{
    /// <summary>
    /// Категория условия пропуска
    /// </summary>
    public enum AllowedEnum
    {
        [Description("Пропуск невозможен")]
        Denied,
        [DescriptionAttribute("Пропуск возможен")]
        Allowed,
        [DescriptionAttribute("Пропуск возможен с ограничением")]
        Restricted,
    }
}
