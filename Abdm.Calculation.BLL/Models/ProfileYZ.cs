using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Кривая в плоскости YZ, обозначающая поверхность влияния в этой плоскости
    /// </summary>
    public class ProfileYZ
    {
        /// <summary>
        /// значение по оси X - индентификатор профиля
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// точки, через которые проходит кривая, отсортированные по первому значению вектора. 
        /// По краям зануляется точками, значение которых равно нулю. 
        /// </summary>
        public required SortedList<double, Vector2D> Vectors { get; set; }

        /// <summary>
        /// Экстремумы кривой. У валидных профилей не бывает пустым. 
        /// </summary>
        public required Vector2D[] Extremums { get; set; }

        /// <summary>
        /// индексы экстремумов-максимумов. 
        /// Так как график по краям зануляется, то у него всегда будет хотя бы один максимум. 
        /// Если максимума нет, то это возможно только, если весь график в отрицательной зоне -> тогда график невалидный, его проверять смысла нет.
        /// </summary>
        public required int[] MaximumIndexes { get; set; }
    }
}
