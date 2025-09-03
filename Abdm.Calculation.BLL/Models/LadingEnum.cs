using System.ComponentModel;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Тип нагрузки
    /// </summary>
    public enum LadingEnum
    {
        [Description("Пользовательская")]
        User = 0,
        [Description("А8")]
        A8 = 10,
        [Description("А11")]
        A11 = 20,
        [Description("А14")]
        A14 = 30,
        [Description("Н11 (НК-80)")]
        N11 = 40,
        [Description("Н14 (НК-100)")]
        N14 = 50,
        [Description("АБ-51")]
        AB51 = 60,
        [Description("АБ-74")]
        AB74 = 70,
        [Description("АБ-151")]
        AB151 = 80,
        [Description("Н-10")]
        N_10 = 90,
        [Description("Н-13")]
        N_13 = 100,
        [Description("Н-18")]
        N_18 = 110,
        [Description("Н-30")]
        N_30 = 120,
        [Description("НГ-60 (Т-60/5)")]
        NG60 = 130,
        [Description("НГ-30 (Т-30/4)")]
        NG30 = 140,
        [Description("Т-60")]
        T60 = 150,
        [Description("Т-25")]
        T25 = 160,
        [Description("ЭНз")]
        EN3 = 170
    }
}
