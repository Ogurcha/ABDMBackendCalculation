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
            if (Formulas.IsOdd(load.Axles.Length) && !equalityComparer.Equals(load.Axles[mid].Position, load.Length / 2))
            {
                return false;
            }
            for (int i = 0; i < mid; i++)
            {
                var a1 = load.Axles[i];
                var a2 = load.Axles[^(i+1)];

                if (!equalityComparer.Equals(a1.Position, load.Length - a2.Position)
                    || !a1.WheelsDistance.SequenceEqual(a2.WheelsDistance))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Считает направления, по которым надо прогонять ТС, исходя из симметрии нагрузки и параметров прогона
        /// </summary>
        /// <returns>[true, false] = прогонять и туда и обратно, [true] только вперёд, [false] только обратно</returns>
        public bool[] CalculateDirection(bool? isSymmetric, Enums.DriveDirectionEnum directionEnum)
        {
            if (!isSymmetric!.Value && directionEnum == Enums.DriveDirectionEnum.Bidirection)
            {
                return [true, false];
            }
            else if (directionEnum == Enums.DriveDirectionEnum.Backward)
            {
                return [false];
            }
            else
            {
                return [true];
            }
        }
    }
}
