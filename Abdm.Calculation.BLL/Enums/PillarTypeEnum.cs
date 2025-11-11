using System.ComponentModel;

namespace Abdm.Calculation.BLL.Enums
{
    public enum PillarTypeEnum
    {
        Other = 0,

        [Description("концевая обсыпная")]
        EndBackfill = 10,

        [Description("концевая необсыпная")]
        EndNonBackfill = 20,

        [Description("промежуточная")]
        Intermediate = 30,

        [Description("пилон висячей (вантовой) конструкции")]
        SuspendedCableStayedStructurePylon = 40,

        [Description("анкерная опора висячей (вантовой) конструкции")]
        SuspendedCableStayedStructureAnchorSupport = 50,
    }
}
