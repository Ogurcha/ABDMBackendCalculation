using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Maths.Extensions;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Services.LowLevelCalculation
{
    public class VehicleStrainProviderVolumetric(IEqualityComparer<double> equalityComparer) : VehicleStrainProvider(), IVehicleStrainProvider
    {
        protected override WheelStrain GetWheelStrain(ProfileYZ profilebase, Dictionary<ProfileYZ, HashSet<Interval>> positivePiecesMap, Axle axle, double Y)
        {
            if (profilebase is not ProfileYZExtended profile)
            {
                return GetWheelStrain(profilebase, positivePiecesMap, axle, Y);
            }
            if (profilebase.MaximumIndexes.Length == 0 || profilebase.PositivePieceMap.Count() == 0)
            {
                return new WheelStrain
                {
                    Position = new Vector2D
                    {
                        X = profilebase.X,
                        Y = Y
                    },
                    AxleRef = axle,
                    Strain = 0d,
                    ZValue = 0d,
                };
            }
            ;
            var zValue = GetVolumetricZValueByY(profile, axle, Y, out (Interval? i1, Interval? i2) positivePieces);
            var strain = zValue * axle.WheelWeight;
            var wheel = new WheelStrain
            {
                Position = new Vector2D
                {
                    X = profile.X,
                    Y = Y
                },
                AxleRef = axle,
                Strain = strain,
                ZValue = zValue,
                FootprintLength = profile.FootprintLength[axle],
                FootprintWidth = profile.FootprintWidth[axle],
            };
            if (positivePieces.i1 != null)
            {
                positivePiecesMap[profile].Add(positivePieces.i1);
            }
            if (positivePieces.i2 != null)
            {
                positivePiecesMap[profile].Add(positivePieces.i2);
            }

            return wheel;
        }

        /// <summary>
        /// Рассчет значения поверхности влияния на профиле используя расчёты объёмов поверхности
        /// </summary>
        /// <param name="positivePieces">позитивные отрезки профиля, на которых искали напряжение. 
        /// Может вернуть null'ы, если рассчёт происходил в отрицательной зоне профиля></param>
        public virtual double GetVolumetricZValueByY(
            ProfileYZExtended profile,
            Axle axle,
            double Y,
            out (Interval? i1, Interval? i2) positivePieces)
        {
            var radius = profile.FootprintLength[axle] / 2;
            var areaCenter = CalculateZAreaAroundY(profile.SortedVectors, Y, radius, out var indexesCenter);

            double totalVolume = 0d;
            double? previousArea = null; double? previousPosition = null;
            double currentArea; double curentPosition;
            for (int i = 0; i < profile.VolumetricProfiles.Count; i++)
            {
                if (profile.VolumetricProfiles[axle][i].X.Equals(profile.X))
                {
                    currentArea = areaCenter;
                    curentPosition = profile.X;
                }
                else
                {
                    currentArea = CalculateZAreaAroundY(profile.VolumetricProfiles[axle][i].SortedVectors, Y, radius, out _);
                    curentPosition = profile.VolumetricProfiles[axle][i].X;
                }
                if (previousArea != null)
                {
                    totalVolume += MathExtensions.FrustrumVolume(curentPosition - previousPosition!.Value, previousArea.Value, currentArea);
                }
                previousArea = currentArea;
                previousPosition = curentPosition;
            }

            positivePieces =
                (profile.PositivePieceMap.TryGetValue(profile.SortedVectors[indexesCenter.indexLeft].X, out Interval? i1) ? i1 : null,
                profile.PositivePieceMap.TryGetValue(profile.SortedVectors[indexesCenter.indexRight].X, out Interval? i2) ? i2 : null);

            return totalVolume / profile.FootprintWidth[axle] / profile.FootprintLength[axle];
        }

        /// <summary>
        /// Предполагая, что на поверхность давит не единичный вектор - а полоска с определённой протяжённостью и центром. 
        /// Считает площадь под поверхностью, возникающую в результате давления
        /// </summary>
        protected virtual double CalculateZAreaAroundY(Vector2D[] vectors,
            double Y,
            double radius,
            out (int indexLeft, int indexRight) indexes)
        {
            var YStart = Y - radius;
            var YFinish = Y + radius;

            var edgeLeft = vectors.FindBetweenIndexes(YStart, (v) => v.X, equalityComparer);
            var edgeRight = vectors.FindBetweenIndexes(YFinish, (v) => v.X, equalityComparer);

            var indexStart = (edgeLeft.Left ?? -1) + 1;
            var indexFinish = edgeRight.Right ?? vectors.Length;

            var trapezoidVectors = vectors
                .Skip(indexStart)
                .Take(indexFinish - indexStart);

            if (edgeLeft.Left != edgeLeft.Right && edgeLeft.Left != null && edgeLeft.Right != null)
            {
                var z1 = Formulas.GetOrdinat(vectors[edgeLeft.Left.Value],
                vectors[edgeLeft.Right.Value],
                YStart);
                var firstVector = new Vector2D(YStart, z1);
                trapezoidVectors = trapezoidVectors.Prepend(firstVector);
            }

            if (edgeRight.Left != edgeRight.Right && edgeRight.Left != null && edgeRight.Right != null)
            {
                var z2 = Formulas.GetOrdinat(vectors[edgeRight.Left.Value],
                    vectors[edgeRight.Right.Value],
                    YFinish);
                var lastVector = new Vector2D(YFinish, z2);
                trapezoidVectors = trapezoidVectors.Append(lastVector);
            }

            indexes = (edgeLeft.Left ?? 0, edgeRight.Right ?? vectors.Length - 1);

            return MathExtensions.CalculateAreaUnderCurve(trapezoidVectors.ToArray());
        }
    }
}
