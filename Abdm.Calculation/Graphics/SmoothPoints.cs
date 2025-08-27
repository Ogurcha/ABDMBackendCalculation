using System.Collections.Generic;
using g4;

namespace Abdm.Calculation.Graphics
{
    /// <summary>
    /// Список гладких (плавных) точек кривой.
    /// Ключи - это точки, через которые проходит кривая
    /// значения - это пара углов, под которой должна пройти касательная в этих точках, чтобы получилась кривая
    /// </summary>
    public class SmoothPoints : Dictionary<Vector3d, double[]>
    {
    }
}
