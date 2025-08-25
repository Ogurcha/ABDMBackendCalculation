namespace Abdm.Calculation.Models
{
    public class Roadway
    {
        /// <summary>
        /// Количество путей у моста
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Максимальное возвышение профиля моста
        /// </summary>
        public float RoadHeight { get; set; }

        /// <summary>
        /// Отступ слева
        /// </summary>
        public float LeftSafeline { get; set; }

        /// <summary>
        /// Отступ справа
        /// </summary>
        public float RightSafeline { get; set; }

        /// <summary>
        /// Сдвиг позиции по X координате. Если точки начинаются не в нуле по иксу, то сдвиг нужен для компенсации
        /// </summary>
        public float PositionShift { get; set; }
    }
}
