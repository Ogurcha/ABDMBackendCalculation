using System.ComponentModel;

namespace Abdm.Calculation.BLL.Enums
{
    public enum SuperStructureTypeEnum
    {
        Other = 0,

        [Description("балки ребристые c диафрагмами")]
        RibbedBeamsWithDiaphragms = 10,

        [Description("балки ребристые без диафрагм")]
        RibbedBeamsWithoutDiaphragms = 20,

        [Description("балки П-образные")]
        PShapedBeams = 25,

        [Description("балки U-образные")]
        UShapedBeams = 26,

        [Description("балки прокатные")]
        RolledBeams = 30,

        [Description("балки со сплошной стенкой")]
        BeamsWithSolidWeb = 40,

        [Description("балки клееные")]
        GluedLaminatedBeams = 45,

        [Description("балки подпруженные аркой")]
        BeamsSupportedByAnArch = 50,

        [Description("плитные")]
        Slab = 60,

        [Description("плитно-ребристые")]
        SlabRibbed = 65,

        [Description("фермы сквозные")]
        ThroughTrusses = 70,

        [Description("фермы сквозные с открыт. верх. поясом")]
        ThroughTrussesWithOpenTopChord = 80,

        [Description("фермы Тауна")]
        TownTrusses = 90,

        [Description("фермы Гау-Журавского")]
        GauZhuravskyTrusses = 100,

        [Description("фермы ригельно-подкосные")]
        BeamStrutTrusses = 110,

        [Description("коробка (в т.ч. с промеж. стенками)")]
        BoxIncludingWithIntermediateWalls = 120,

        [Description("коробки раздельные")]
        SeparateBoxes = 130,

        [Description("свод с надсводным строением")]
        VaultWithSuperVaultStructure = 140,

        [Description("свод с засыпкой")]
        VaultWithBackfill = 142,

        [Description("арки")]
        Arches = 150,

        [Description("арки с жесткой балкой")]
        RigidBeamArches = 160,

        [Description("арки с надарочным строением")]
        ArchesWithAStaircase = 170,

        [Description("прогоны простые")]
        SimpleGirders = 180,

        [Description("прогоны составные")]
        CompositeGirders = 190,
    }
}
