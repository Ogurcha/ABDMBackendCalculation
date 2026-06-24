using AisIssoEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisPcCore
{



    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    class AllowNKAttribute : Attribute
    {
        public ais7EnumS_TYPNK[] NK { get; set; }
        public AllowNKAttribute(ais7EnumS_TYPNK nk) { NK = new ais7EnumS_TYPNK[] { nk }; }
        public AllowNKAttribute(ais7EnumS_TYPNK nk1, ais7EnumS_TYPNK nk2) { NK = new ais7EnumS_TYPNK[] { nk1, nk2 }; }
        public AllowNKAttribute(ais7EnumS_TYPNK[] nk) { NK = nk; }
    }



    public enum CpSubType
    {
        stNone = 0,

        [AllowNK(ais7EnumS_TYPNK.TypNk_PS, ais7EnumS_TYPNK.TypNk_Opora)]
        [Description("Железобетонный элемент. Плоский изгиб. Прочность сечения")]
        st10 = 10,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Железобетонная плита проезжей части. Прочность сечения плиты при местной нагрузке (инженерный расчет)")]
        st12 = 12,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Железобетонная плита проезжей части. Прочность сечения плиты при местной нагрузке")]
        st14 = 14,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS, ais7EnumS_TYPNK.TypNk_Opora)]
        [Description("Металлический элемент. Плоский изгиб. Прочность сечения")]
        st20 = 20,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS, ais7EnumS_TYPNK.TypNk_Opora)]
        [Description("Металлический элемент. Плоский изгиб. Устойчивость сжатого пояса")]
        st22 = 22,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS, ais7EnumS_TYPNK.TypNk_Opora)]
        [Description("Металлический элемент. Плоский изгиб. Прочность соединения составного сечения")]
        st24 = 24,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS, ais7EnumS_TYPNK.TypNk_Opora)]
        [Description("Металлический элемент. Осевая сила. Прочность сечения")]
        st30 = 30,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS, ais7EnumS_TYPNK.TypNk_Opora)]
        [Description("Металлический элемент. Осевое сжатие. Общая устойчивость")]
        st32 = 32,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Сталежелезобетонный элемент. Прочность при плоском изгибе")]
        st40 = 40,

        #region Ращ. "Железобетонный элемент. Внецентренное сжатие
        [AllowNK (ais7EnumS_TYPNK.TypNk_Opora)]
        [Description ("Железобетонный элемент. Внецентренное сжатие. Прочность сечения")]       
        st50 = 50,
        [AllowNK(ais7EnumS_TYPNK.TypNk_Opora)]
        [Description ("Железобетонный элемент. Внецентренное сжатие. Общая устойчивость")]      
        st60 = 60,
        #endregion Ращ. "Железобетонный элемент. Внецентренное сжатие

        [AllowNK (ais7EnumS_TYPNK.TypNk_Opora)]
        [Description("По сопоставлению воздействий")]
        st70 = 70,
        [AllowNK(ais7EnumS_TYPNK.TypNk_OpCH)]
        [Description("Прочность конструкции")]
        st80 = 80,
        [AllowNK(ais7EnumS_TYPNK.TypNk_OpCH)]
        [Description("Линейное перемещение")]
        st90 = 90,
        [AllowNK(ais7EnumS_TYPNK.TypNk_OpCH)]
        [Description("Угол поворота")]
        st100 = 100,

        #region Ращ. Ортотропная плита проезжей части:
        [AllowNK (ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Ортотропная плита проезжей части. Прочность продольного ребра (в зоне положительных моментов)")]
        st510 = 510,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Ортотропная плита проезжей части. Прочность продольного ребра (в зоне отрицательных моментов)")]
        st520 = 520,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Ортотропная плита проезжей части. Прочность поперечного ребра (балки)")]
        st530 = 530,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Ортотропная плита проезжей части. Местная устойчивость неподкрепленного ребра (полки)")]
        st553 = 553,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Ортотропная плита проезжей части. Местная устойчивость подкрепленного ребра (стенки)")]
        st556 = 556,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Ортотропная плита проезжей части. Местная устойчивость листа настила")]
        st558 = 558,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Ортотропная плита проезжей части. Прочность листа настила")]
        st540 = 540,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Ортотропная плита проезжей части. Общая устойчивость")]
        st560 = 560,
        #endregion Ращ. Ортотропная плита проезжей части:

        [AllowNK (ais7EnumS_TYPNK.TypNk_PS, ais7EnumS_TYPNK.TypNk_Opora)]
        [Description("Деревянный элемент. Плоский изгиб. Прочность сечения")]
        st610 = 610,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS, ais7EnumS_TYPNK.TypNk_Opora)]
        [Description("Деревянный элемент. Осевая сила. Прочность сечения")]
        st630 = 630,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS, ais7EnumS_TYPNK.TypNk_Opora)]
        [Description("Деревянный элемент. Осевое сжатие. Общая устойчивость")]
        st632 = 632,

        #region Ращ. Местная устойчивость стенок главных балок:
        [AllowNK (ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Устойчивость стенок без продольных ребер жесткости. Сжато-растянутые пластинки")]
        st710 = 710,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Устойчивость стенок с одним продольным ребром жесткости. Преимущественно растянутые пластинки")]
        st720 = 720,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Устойчивость стенок с одним продольным ребром жесткости. Преимущественно сжатые пластинки")]
        st730 = 730,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Устойчивость стенок с двумя и более продольными ребрами жесткости. Пластинки в растянутой зоне")]      
        st740 = 740,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Устойчивость стенок с двумя и более продольными ребрами жесткости. Сжато-растянутые пластинки")]
        st760 = 760,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Устойчивость стенок с двумя и более продольными ребрами жесткости. Пластинки в сжатой зоне")]      
        st770 = 770,
        [AllowNK(ais7EnumS_TYPNK.TypNk_PS)]
        [Description("Устойчивость стенок при сжатии по всей высоте")]
        st790 = 790,
        #endregion Ращ. Местная устойчивость стенок главных балок:

    }

}
