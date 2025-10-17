using System.ComponentModel;

namespace Abdm.Calculation.DAL.Enums
{
    /// <summary>
    /// тип чекпоинта - балка или опора
    /// </summary>
    public enum CheckPointTypeEnum
    {
        [Description("Пролетное строение")]
        TypNk_PS = 10,

        [Description("Опорные части")]
        TypNk_OpCH = 20,

        [Description("Опора")]
        TypNk_Opora = 30,

        [Description("Фундамент")]
        TypNk_Fund = 40,

        [Description("Основание")]
        TypNk_Osnov = 50
    }
}
