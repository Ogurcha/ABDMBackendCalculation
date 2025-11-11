using System.ComponentModel;

namespace Abdm.Calculation.BLL.Enums
{
    public enum StaticSystemTypeEnum
    {
        Other = 0,

        [Description("балочная разрезная")]
        SplitBeam = 10,

        [Description("балочная неразрезная")]
        ContinuousBeam = 20,

        [Description("балочная температурно-неразрезная")]
        ThermallyContinuousBeam = 30,

        [Description("балочная одноконсольная")]
        SingleCantileverBeam = 40,

        [Description("балочная двухконсольная")]
        DoubleCantileverBeam = 50,

        [Description("рамная")]
        Frame = 60,

        [Description("рамно-консольная")]
        FrameCantilever = 70,

        [Description("арочная безраспорная")]
        ArchedNonStrut = 80,

        [Description("арочная бесшарнирная")]
        HingedLessArch = 90,

        [Description("арочная одношарнирная")]
        SingleHingedArch = 100,

        [Description("арочная двухшарнирная")]
        DoubleHingedArch = 110,

        [Description("арочная трехшарнирная")]
        ThreeHingedArch = 120,

        [Description("комбинированная")]
        Combined = 130,

        [Description("висячая")]
        Hanging = 140,

        [Description("вантовая")]
        CableStayed = 150,

        [Description("ригельно-подкосная")]
        BeamStrut = 160,

        [Description("подкосная")]
        Strut = 170,
    }
}
