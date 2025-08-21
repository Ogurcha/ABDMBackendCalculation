using System.ComponentModel;

namespace Abdm.Calculation.Models
{
    public enum CheckPointEnum
    {
        [Description("[нет данных]")]
        None = 0,
        [Description("По изгибающему моменту М, тс*м")]
        BeamM = 10,
        [Description("По поперечной силе Q, тс")]
        BeamQ = 20,
        [Description("По продольному усилию N, тс")]
        LongitudinalForce = 30,
        [Description("По нормальному напряжению, тс/м2")]
        NormalStress = 40,
        [Description("По касательному напряжению, тс/м2")]
        ShearStress = 50,


        [Description("Опорная реакция N, тс")]
        SupportReaction = 60,
        [Description("Допустимый диапазон, м")]
        AvRangeDistance = 70,
        [Description("Допустимый диапазон, рад")]
        AvRangeAngle = 80,

        [Description("По изгибающему моменту М в плите, тс*м")]
        PlateM,
        [Description("По поперечной силе Q в плите, тс")]
        PlateQ

    }
}
