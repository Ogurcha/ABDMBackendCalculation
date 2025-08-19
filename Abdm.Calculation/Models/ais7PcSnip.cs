using System.ComponentModel;

namespace Abdm.Calculation.Models
{
    public enum ais7PcSnip
    {
        [Description("Гушосдор НКВД 1938")]
        sn38 = 0,
        [Description("Гушосдор МВД 1943")]
        sn43 = 1,
        [Description("Гушосдор МВД 1948 (+Н 106-53)")]
        sn48 = 2,
        [Description("Н 106-53 (НиТУ-128-55)")]
        sn53 = 3,
        [Description("СН 200-62")]
        sn62 = 4,
        [Description("СНиП 2.05.03-84")]
        sn84 = 5,
        [Description("СП 35.13330.2011")]
        sp35 = 6,
        [Description("ОДМ 218.4.025-2016")]
        odm16 = 7,

        [Description("ГОСТ 32960-2014")]
        gost2014 = 8,

    }
}
