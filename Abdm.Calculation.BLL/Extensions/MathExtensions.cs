using Abdm.Calculation.BLL.Models.Primitives;

namespace Abdm.Calculation.BLL.Extensions
{
    public static class MathExtensions
    {
        /// <summary>
        /// Возвращает позитивные интервалы у двумерной функции. 
        /// Кейсы с касательным с нулём не учитываются.
        /// </summary>
        public static IEnumerable<Vector2D> GetPositveIntervals(IList<Vector2D> function)
        {
            if (function.FirstOrDefault() is Vector2D first)
            {
                var sign = Math.Sign(first.Y);
                var intervalStart = first.X;

                for (int i = 1; i < function.Count; i++)
                {
                    var newSign = Math.Sign(function[i].Y);
                    if (newSign == sign)
                    {
                        continue;
                    }
                    if (newSign < 0)
                    {
                        yield return new Vector2D(intervalStart, function[i].X);
                    }
                    sign = newSign;
                    intervalStart = function[i].X;
                }

                if (sign > 0 && intervalStart != function.Last().X)
                {
                    yield return new Vector2D(intervalStart, function.Last().X);
                }
            }
        }
    }
}
