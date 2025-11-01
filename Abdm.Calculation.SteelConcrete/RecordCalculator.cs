using Abdm.Calculation.Maths.Models;
using Abdm.Calculation.SteelConcrete.Models;

namespace Abdm.Calculation.SteelConcrete
{
    internal static class RecordCalculator
    {
        internal static double GetN(RecordContainer recordContainer, Double a, Double x, bool isNegative)
        {
            a = !Double.IsNaN(a) ? Math.Max(0, Math.Min(1, a)) : 0;
            var i1 = (Int32)Math.Truncate(a / 0.2);
            var y1 = GetN(recordContainer.Records[Math.Min(recordContainer.Records.Length - 1, Math.Max(i1, 0))], x, isNegative);
            var y2 = GetN(recordContainer.Records[Math.Min(recordContainer.Records.Length - 1, Math.Max(i1 + 1, 0))], x, isNegative);
            var a1 = recordContainer.Records[Math.Min(recordContainer.Records.Length - 1, Math.Max(i1, 0))].As;
            var a2 = recordContainer.Records[Math.Min(recordContainer.Records.Length - 1, Math.Max(i1 + 1, 0))].As;

            return y1 == y2 ? y1 : (a - a1) / (a2 - a1) * (y2 - y1) + y1;
        }

        internal static double GetN(Record record, double x, bool isNegative)
        {
            if (x <= 0) return GetN(record.Vectors[0], isNegative);
            if (x >= 0.7) return GetN(record.Vectors[^1], isNegative);
            var i1 = (int)Math.Truncate(x / 0.05);
            var x1 = i1 * 0.05;
            var x2 = (i1 + 1) * 0.05;
            var y1 = GetN(record.Vectors[i1], isNegative);
            var y2 = GetN(record.Vectors[i1 + 1], isNegative);
            return (x - x1) / (x2 - x1) * (y2 - y1) + y1;
        }

        internal static double GetN(Vector2D v, bool isNegative) => isNegative ? v.X : v.Y;
    }
}
