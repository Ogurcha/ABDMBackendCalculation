using Abdm.Calculation.BLL.Models.Primitives;

namespace Abdm.Calculation.BLL.Extensions
{
    public static class MathExtensions
    {
        /// <summary>
        /// Возвращает позитивные интервалы у двумерной функции. 
        /// Кейсы с касательным с нулём не учитываются.
        /// </summary>
        public static IEnumerable<Vector2D> GetPositvePieces(IList<Vector2D> function)
        {
            if (function.FirstOrDefault() is Vector2D first)
            {
                bool insidePositiveRegion = false;
                int intervalStartIndex = -1;

                for (int i = 0; i < function.Count; i++)
                {
                    if (!insidePositiveRegion && function[i].Y > 0)
                    {
                        insidePositiveRegion = true;
                        intervalStartIndex = i;
                    }
                    else if (insidePositiveRegion &&
                            (i == function.Count - 1 || function[i + 1].Y <= 0))
                    {
                        insidePositiveRegion = false;
                        yield return new Vector2D(
                            intervalStartIndex > 0 
                            ? GetIntersectionWithY(function[intervalStartIndex], function[intervalStartIndex - 1])!.Value
                            : function[intervalStartIndex].X
                            , i < function.Count - 1 
                            ? GetIntersectionWithY(function[i], function[i+1])!.Value
                            : function[i].X);
                    }
                }
            }
        }

        public static double? GetIntersectionWithY(Vector2D start, Vector2D finish)
        {
            double deltaX = finish.X - start.X;

            if (deltaX == 0)
                return null;

            double xIntersect = start.X +
                             (0 - start.Y) * deltaX /
                             (finish.Y - start.Y);

            return xIntersect;
        }
    }
}
