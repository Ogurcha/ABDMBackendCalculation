namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Точка профиля, в которой достигается экстремум (максима или минимума) в зависимости от значения <see cref="isMaximum"/>
    /// </summary>
    public struct ProfileExtremum
    {
        /// <summary>
        /// Позиция экстремума в <see cref="ProfileYZ"/>
        /// </summary>
        public double Position { get; set; }

        /// <summary>
        /// Точка максимума или минимума
        /// </summary>
        public bool isMaximum { get; set; }
    }
}
