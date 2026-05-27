using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Модель нагрузки
    /// </summary>
    public class LoadModel
    {
        /// <summary>
        /// Ширина ТС
        /// </summary>
        public required double Width { get; set; }

        /// <summary>
        /// Длина ТС
        /// </summary>
        public required double Length { get; set; }

        /// <summary>
        /// Расстояние до следующего ТС в транспорнтной колонне
        /// </summary>
        public required double Distance { get; set; }

        /// <summary>
        /// Оси ТС
        /// </summary>
        public required Axle[] Axles { get; set; }

        /// <summary>
        /// Индентичен ли перед нагрузки с его задом
        /// </summary>
        public bool IsSymmetric { get; set; }

        /// <summary>
        /// Направления для прокатки ТС. true - вперед, false - назад. true+false = оба.
        /// </summary>
        public bool[] ActualDirection { get; set; } = [true];

        /// <summary>
        /// минимальное расстояние между транспортными колоннами
        /// </summary>
        public double Interval { get; set; } = NormConstants.MinimalDistanceBetweenTrajectories;

        /// <summary>
        /// Для прицепов, вагонов поезда и т.п.
        /// </summary>
        public LoadModel? SecondaryLoadModel { get; set; }

        /// <summary>
        /// Тип нагрузки
        /// </summary>
        public LoadGroupTypeEnum Type { get; set; }

        /// <summary>
        /// Расстояние от центральной оси ТС до всех возможных его колес С ОДНОЙ СТОРОНЫ
        /// Например, если в параметры передаётся легковушка с двумя <see cref="Axle"/> и четырмя колёсами, 
        /// то вернётся словарь <расстояниеОтКолесаДоЦентра, 2>. Вернётся только одно значение, так как переднее и заднее колесо на одинаковом расстоянии. Число два означает, что на таком расстоянии оба значения. 
        /// Второе значение тапла - это вес колёс на таком расстоянии
        /// </summary>
        public Dictionary<double, (int, double)>? WheelOffsetsMap { get; set; }

        /// <summary>
        /// То же самое, что и <see cref="WheelOffsetsMap"/>, только расстояние до ЦЕНТРОВ колес, а не до ближнего края колеса
        /// </summary>
        public Dictionary<double, int>? WheelOffsetsMapCentered { get; set; }
    }
}
