using System.ComponentModel;

namespace Abdm.Calculation.DAL.Enums
{
    /// <summary>
    /// тип чекпоинта - балка или опора
    /// </summary>
    public enum CheckPointTypeEnum
    {
        [Description("Пролетное строение")]
        Surface = 10,

        [Description("Опорные части")]
        PillarParts = 20,

        [Description("Опора")]
        Pillar = 30,

        [Description("Фундамент")]
        Foundation = 40,

        [Description("Основание")]
        Basement = 50
    }
}
