using System.ComponentModel;

namespace AisIssoEnum
{
    public enum ais7EnumS_TYP
    {
        [Description("Федеральная")]
        Federal = 1,
        [Description("Территориальная рег.значения")]
        TerritorialRegional = 2,
        [Description("Территориальная обл.значения")]
        TerritorialObl = 3,
        [Description("Территориальная мест.значения")]
        TerritorialLocal = 4,
        [Description("Внутрихозяйственная")]
        OnFarm = 5,
        [Description("Городские дороги и улицы")]
        City = 50,
    }


    public enum ais7EnumS_TYPOD
    {
        Multi = 10,
        Double = 20,
        Single = 30,
        None = 40
    }

    public enum ais7EnumS_TYPIZOL
    {
        type_10 = 10,
        type_20 = 20,
        type_30 = 30,
        type_40 = 40,
        type_50 = 50,
        type_60 = 60,
        None = 70
    }

    public enum ais7EnumS_OTC_EXP
    {
        [Description("аварийное")]
        Emergency = 0,
        [Description("предаварийное")]
        BeforeEmergency = 1,
        [Description("неудовлетворительное")]
        Unsatisfactory = 2,
        [Description("удовлетворительное")]
        Middling = 3,
        [Description("хорошее")]
        Good = 4,
        [Description("отличное")]
        Perfect = 5,
        [Description("не оценивалось")]
        NotRated = 6,

        [Description("оценке не подлежит")]
        RateImposible = 10,
        [Description("оценка отсрочена")]
        RateDelayed = 11,


        [Description("аварийное (неактуально)")]
        EmergencyNA = 200,
        [Description("предаварийное (неактуально)")]
        BeforeEmergencyNA = 201,
        [Description("неудовлетворительное (неактуально)")]
        UnsatisfactoryNA = 202,
        [Description("удовлетворительное (неактуально)")]
        MiddlingNA = 203,
        [Description("хорошее (неактуально)")]
        GoodNA = 204,
        [Description("отличное (неактуально)")]
        PerfectNA = 205,
    }

    public enum ais7EnumS_FENCE
    {
        [Description("металлические сквозные секционные")]
        type_10 = 10,
        [Description("металлические сквозные бесстоечные")]
        type_20 = 20,
        [Description("металлические, совмещенные с ОБ")]
        type_25 = 25,
        [Description("металлические сплошностенчатые")]
        type_30 = 30,
        [Description("ж/б поручень с металлической решеткой")]
        type_40 = 40,
        [Description("железобетонные сквозные")]
        type_50 = 50,
        [Description("железобетонные сплошностенчатые")]
        type_60 = 60,
        [Description("деревянные")]
        type_70 = 70,
        [Description("в составе защитной галереи")]
        type_75 = 75,
        [Description("элементы несущей конструкции")]
        type_76 = 76,
        [Description("отсутствуют")]
        None = 80
    }

    public enum ais7EnumS_WHE_SPEED
    {
        [Description("отсутствуют")]
        None = 10,
        [Description("слева")]
        AtLeft = 20,
        [Description("справа")]
        AtRight = 30,
        [Description("с обеих сторон")]
        AtBothSides = 40
    }

    public enum ais7EnumS_TYPCROSS
    {
        [Description("основная дорога")]
        Main = 10,
        [Description("левое примыкание")]
        Left = 20,
        [Description("правое примыкание")]
        Right = 30,
        [Description("съезд транспортной развязки")]
        Brunch = 40,
        [Description("разворотная петля")]
        Loop = 50
    }

    public enum ais7EnumS_TYPOBSL
    {
        [Description("обследование")]
        Survey = 10,
        [Description("диагностика")]
        Diagnosis = 20,
        [Description("периодический осмотр")]
        PeriodicInspection = 30,
        [Description("постоянный надзор")]
        Monitoring = 40,
        [Description("специальные наблюдения")]
        SpecialObservation = 50,
        [Description("испытание")]
        Test = 60,
        [Description("нет необходимости")]
        NotNecessary = 70
    }

    public enum ais7EnumS_TYPOG
    {
        [Description("барьерное")]
        type10 = 10,
        [Description("барьерное двустороннее")]
        type10a = -10,
        [Description("барьерное на цоколе")]
        type15 = 15,
        [Description("бордюрное")]
        type20 = 20,
        [Description("парапетное")]
        type30 = 30,
        [Description("парапетное с поручнем")]
        type35 = 35,
        [Description("комбинированное")]
        type40 = 40,
        [Description("тросовое (цепное)")]
        type50 = 50,
        [Description("колесоотбойный брус")]
        type60 = 60,

        [Description("Центральная разделительная полоса без ограждения")]
        LineWOaFence = 120,
        [Description("Отсутствует")]
        None = 130
    }

    public enum ais7EnumS_TYPPS
    {
        [Description("балки ребристые c диафрагмами")]
        BeamTypeI = 10,
        [Description("балки ребристые без диафрагм")]
        BeamTypeI_2 = 20,
        [Description("балки П-образные")]
        BeamTypeIII = 25,
        [Description("плитные")]
        BeamTypeIV = 60,
        [Description("тип не определен")]
        Unknown = 0
    }

    public enum ais7EnumS_TYPPREP
    {
        [Description("постоянный водоток")]
        PV = 10,
        [Description("периодический водоток")]
        VV = 30,
        [Description("скотопрогон")]
        Underpasses = 40,
        [Description("автомобильная дорога")]
        AD = 50,
        [Description("железная дорога")]
        GD = 60,
        [Description("пешеходный проход")]
        Crosswalk = 70,
        [Description("землевладения")]
        Tenure = 80,
        [Description("коллектор")]
        Collector = 90,
        [Description("горный массив")]
        Massif = 100,
        [Description("крутой откос ")]
        Scarp = 110
    }

    public enum ais7EnumS_NAPRAV
    {
        [Description("встречное")]
        Bidirection = 10,
        [Description("по ходу км")]
        Forward = 20,
        [Description("против хода км")]
        Backward = 30,
    }

    public enum ais7EnumS_TYPNK
    {
        [Description("Пролетное строение")]
        TypNk_PS = 10,
        [Description("Опорные части")]
        TypNk_OpCH = 20,
        [Description("Опора")]
        TypNk_Opora = 30,
        [Description("Фундамент")]
        TypNk_Fund = 40,
        [Description("Основание")]
        TypNk_Osnov = 50
    }
}
