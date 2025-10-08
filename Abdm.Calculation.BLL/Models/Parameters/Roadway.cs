namespace Abdm.Calculation.BLL.Models.Parameters
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
        public double RoadHeight { get; set; }

        /// <summary>
        /// Отступ слева
        /// </summary>
        public double LeftSafeline { get; set; }

        /// <summary>
        /// Отступ справа
        /// </summary>
        public double RightSafeline { get; set; }

        /// <summary>
        /// Сдвиг позиции по X координате. Если точки начинаются не в нуле по иксу, то сдвиг нужен для компенсации
        /// </summary>
        public double PositionShift { get; set; }
    }
}
