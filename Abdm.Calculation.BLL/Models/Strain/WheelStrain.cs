using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Models.Strain
{
    /// <summary>
    /// Итоговое напряжение выдаваемое одним колесом
    /// одного авто
    /// в определённых координатах 
    /// на определённом сооружении
    /// </summary>
    public class WheelStrain
    {
        /// <summary>
        /// Координаты колеса
        /// </summary>
        public required Vector2D Position { get; set; }

        /// <summary>
        /// Напряжение от колеса
        /// </summary>
        public required double Strain { get; set; }

        /// <summary>
        /// Ссылка на ось
        /// </summary>
        public required Axle AxleRef { get; set; }

        /// <summary>
        /// "Нормализованное напряжение" без учёта веса колеса
        /// </summary>
        public double ZValue { get; set; }
    }
}
