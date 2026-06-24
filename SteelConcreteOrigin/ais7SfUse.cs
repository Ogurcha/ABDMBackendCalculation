using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AisPcCore.SfData
{
    public enum ais7SfUse
    {
        [DescriptionAttribute("Поверхность влияния")]
        Single,
        
        [DescriptionAttribute("Конфигурация сечения")]
        CrossSection,
        [DescriptionAttribute("ПВ для момента")]
        Moment,
        [DescriptionAttribute("ПВ для силы")]
        Strength,

        [DescriptionAttribute("Линия влияния")]
        InfluenceLine,

        [Description ("ПВ продольных напряжений при учете совместной работы (σ_xc)")]
        Sxc,
        [Description ("ПВ продольных напряжений при местном действии нагрузки (σ_xp)")]
        Sxp,
        [Description ("ПВ поперечных напряжений при учете совместной работы (σ_yc)")]
        Syc,
        [Description ("ПВ поперечных напряжений при местном действии нагрузки (σ_yp)")]
        Syp,
        [Description ("ПВ касательных напряжений при учете совместной работы (τ_xyc)")]
        Txyc,
        [Description ("ПВ касательных напряжений при местном действии нагрузки (τ_xyp)")]
        Txyp

    }
}
