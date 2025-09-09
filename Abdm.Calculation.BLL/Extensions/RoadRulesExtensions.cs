using System.ComponentModel;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Extensions
{
    public static class RoadRulesExtensions
    {
        [Description("I. \"Общего назначения\" и \"АК\" без заезда на полосу безопасности")]
        public static RoadRules RR1 => new RoadRules()
        {
            IsPedestrianAllowed = true,
            IsDynamicMovement = true,
            HasSafetyLine = false,
            MaxColumnCount = int.MaxValue,
            MaxAutoInColumn = 1,
            MinColumnDistance = 3,
        };

        [Description("I(1). Вариант – не более 2-х грузовиков в колонне")]
        public static RoadRules RR1_1 => new RoadRules()
        {
            IsPedestrianAllowed = true,
            IsDynamicMovement = true,
            HasSafetyLine = false,
            MaxColumnCount = int.MaxValue,
            MaxAutoInColumn = 2,
            MinColumnDistance = 3,
        };

        [Description("II. \"Общего назначения\" и \"АК\" с заездом на полосу безопасности")]
        public static RoadRules RR2 => new RoadRules()
        {
            IsPedestrianAllowed = false,
            IsDynamicMovement = true,
            HasSafetyLine = false,
            MaxColumnCount = 2,
            MaxAutoInColumn = 1,
            MinColumnDistance = 3,
        };

        [Description("II(1). Вариант – не более 2-х грузовиков в колонне")]
        public static RoadRules RR2_1 => new RoadRules()
        {
            IsPedestrianAllowed = false,
            IsDynamicMovement = true,
            HasSafetyLine = false,
            MaxColumnCount = 2,
            MaxAutoInColumn = 2,
            MinColumnDistance = 3,
        };

        [Description("III. \"Одиночная\" без заезда на полосу безоп.")]
        public static RoadRules RR3 => new RoadRules()
        {
            IsPedestrianAllowed = false,
            IsDynamicMovement = true,
            HasSafetyLine = false,
            MaxColumnCount = 1,
            MaxAutoInColumn = 1,
            MinColumnDistance = 3,
        };

        [Description("IV. \"Специальная АБ\" движущаяся")]
        public static RoadRules RR4 => new RoadRules()
        {
            IsPedestrianAllowed = true,
            IsDynamicMovement = true,
            HasSafetyLine = true,
            MaxColumnCount = 1,
            MaxAutoInColumn = 1,
            MinColumnDistance = 3,
        };
    }
}
