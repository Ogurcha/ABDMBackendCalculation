using System.ComponentModel;

namespace Abdm.Calculation.BLL.Enums
{
    /// <summary>
    /// Тип коэффициента
    /// </summary>
    public enum StrainCoefficientTypeEnum
    {
        [Description("Обязательный коеффициент нагрузки")]
        BasicStrain,
        
        [Description("Коеффициент динамического движения")]
        DynamicMovement,

        [Description("Коеффициент для нагрузок А-класса")]
        TrafficJam,
    }
}
