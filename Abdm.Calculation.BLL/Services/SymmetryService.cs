using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Maths.Helpers;

namespace Abdm.Calculation.BLL.Services
{
    public class SymmetryService(IEqualityComparer<double> equalityComparer) : ISymmetryService
    {
        public bool IsLoadSymmetric(LoadModel load)
        {
            int mid = load.Axles.Length / 2;
            if (Formulas.IsOdd(load.Axles.Length) && !equalityComparer.Equals(load.Axles[mid].AbsolutePosition, load.Length / 2))
            {
                return false;
            }
            for (int i = 0; i < mid; i++)
            {
                var a1 = load.Axles[i];
                var a2 = load.Axles[^(i+1)];

                if (!equalityComparer.Equals(a1.AbsolutePosition, load.Length - a2.AbsolutePosition)
                    || !a1.WheelsDistance.SequenceEqual(a2.WheelsDistance))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
