using System.ComponentModel;

namespace Abdm.Calculation.DAL.Enums
{
    /// <summary>
    /// тип проверки на чекпоинте - зависит от типа дефформации
    /// </summary>
    public enum StrainCalculationTypeEnum
    {
        stNone = 0,

        [Description("Железобетонный элемент. Плоский изгиб. Прочность сечения")]
        st10 = 10,

        [Description("Железобетонная плита проезжей части. Прочность сечения плиты при местной нагрузке (инженерный расчет)")]
        st12 = 12,

        [Description("Железобетонная плита проезжей части. Прочность сечения плиты при местной нагрузке")]
        st14 = 14,

        [Description("Сталежелезобетонный элемент. Прочность при плоском изгибе")]
        st40 = 40,

        [Description("По сопоставлению воздействий")]
        st70 = 70,

        [Description("Прочее")]
        Other = 1,
    }
}
