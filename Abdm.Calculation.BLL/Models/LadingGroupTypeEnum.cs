using System.ComponentModel;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Класс типа нагрузки
    /// </summary>
    public enum LadingGroupTypeEnum
    {
        [Description("Колесная общего назначения")]
        Common = 10,
        [Description("Колесная одиночная")]
        Single = 20,
        [Description("Колесная специальная (АБ)")]
        AB = 30,
        [Description("Гусеничная")]
        Track = 40,
        [Description("Класс А")]
        AClass = 50,
        [Description("Класс Н")]
        NClass = 1000,
    }
}
