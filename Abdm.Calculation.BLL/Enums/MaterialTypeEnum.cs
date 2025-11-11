using System.ComponentModel;

namespace Abdm.Calculation.BLL.Enums
{
    public enum MaterialTypeEnum
    {
        Other = 0,

        [Description("бетон")]
        Concrete = 20,

        [Description("бутобетон")]
        RubbleConcrete = 30,

        [Description("железобетон")]
        ReinforcedConcrete = 40,

        [Description("ПН железобетон")]
        PnReinforcedConcrete = 50,

        [Description("металл")]
        Metal = 130,

        [Description("сталежелезобетон")]
        SteelReinforcedConcrete = 180,

        [Description("древесина")]
        Timber = 190,

        [Description("древесина клееная")]
        GluedLaminatedTimber = 200,

        [Description("каменная кладка")]
        Masonry = 220,

        [Description("кирпичная кладка")]
        Brickwork = 270,

        [Description("композит")]
        Composite = 280,

    }
}
