using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisIssoEnum
{
    public enum ais7EnumSRnnm
    {
        [Description("Значение отсутствует")]
        NotDefined = 0,

        [Description("А14, Н14 (СП 35.13330.2011)")]
        A14_N14_sp = 3,
        [Description("А11, Н11 (СП 35.13330.2011)")]
        A11_N11_sp = 4,
        [Description("А14, Н14 (ГОСТ Р 52748-2007)")]
        A14_N14 = 5,
        [Description("А11, Н11 (ГОСТ Р 52748-2007)")]
        A11_N11 = 6,
        [Description("А14, НК-80 (Московские нормы)")]
        A14_NK80 = 7,
        [Description("А11, НК-80 (СНиП 2.05.03-84*)")]
        A11_NK80 = 10,
        [Description("А8, НГ-60 (СНиП 2.05.03-84)")]
        A8_NG60 = 20,
        [Description("Н-30, НК-80 (СН 200-62)")]
        N30_NK80 = 30,
        [Description("Н-10, НГ-60 (СН 200-62)")]
        N10_NG60_Y62 = 31,
        [Description("Н-30, НГ-60 (СН 200-62)")]
        N30_NG60 = 32,
        [Description("Н-18, НК-80 (Н 106-53)")]
        N18_NK80 = 40,
        [Description("Н-13, НГ-60 (Гушосдор 1948, Н 106-53)")]
        N13_NG60 = 50,
        [Description("Н-13, НГ-30 (Гушосдор 1948, Н 106-53)")]
        N13_NG30 = 55,
        [Description("Н-10, НГ-60 (Гушосдор 1948, Н 106-53)")]
        N10_NG60_Y48 = 60,
        [Description("Н-10, НГ-30 (Гушосдор 1948, Н 106-53)")]
        N10_NG30 = 65,
        [Description("Н-10, Т-60/5 (Гушосдор 1943)")]
        N10_T60_Y43 = 66,
        [Description("Н-10, Т-30/4 (Гушосдор 1943)")]
        N10_T30 = 67,
        [Description("Н-13, Т-60 (Гушосдор 1938)")]
        N13_T60 = 68,
        [Description("Н-10, Т-60 (Гушосдор 1938)")]
        N10_T60_Y38 = 69,
        [Description("Н-10, Т-25 (Гушосдор 1938)")]
        N10_T25 = 70,
        [Description("Н-8, НГ-30")]
        N8_NG30 = 75,
        [Description("АБ-51")]
        AB51 = 80,
        [Description("АБ-74")]
        AB74 = 90,
        [Description("АБ-151")]
        AB151 = 92,
        [Description("400 кгс/кв.м (пешеходная)")]
        P400 = 100,
        [Description("300 кгс/кв.м (пешеходная)")]
        P300 = 110,



        [Description("А14, Н14 (ГОСТ 32960-2014)")]
        A14_N14_gost2014 = 120,
        [Description("А11, Н11 (ГОСТ 32960-2014)")]
        A11_N11_gost2014 = 130,



    }
}
