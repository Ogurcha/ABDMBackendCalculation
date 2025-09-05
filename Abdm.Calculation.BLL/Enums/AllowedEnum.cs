using System.ComponentModel;

namespace Abdm.Calculation.BLL.Enums
{
    /// <summary>
    /// Категория условия пропуска
    /// </summary>
    public enum AllowedEnum
    {
        [Description("Пропуск невозможен")]
        Denied,
        [Description("Пропуск возможен")]
        Allowed,
        [Description("Пропуск возможен с ограничением")]
        Restricted,
    }
}
