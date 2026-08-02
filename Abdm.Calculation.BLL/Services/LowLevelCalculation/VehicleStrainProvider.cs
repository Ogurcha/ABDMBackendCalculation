using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;
using g4;

namespace Abdm.Calculation.BLL.Services.LowLevelCalculation
{
    public class VehicleStrainProvider : IVehicleStrainProvider
    {
        /// <summary>
        /// Расчёт напряжения от ТС
        /// </summary>
        /// <param name="trajectory">Траектория по которой двигается ТС</param>
        /// <param name="Y">Точка, в которой считаем напряжение</param>
        /// <param name="load">параметры нагрузки</param>
        /// <param name="invertAxles">ТС едет задом наперёд</param>
        /// <returns></returns>
        public virtual VehicleStrain GetStrainOnTrajectory(VehicleTrajectory trajectory,
            double Y,
            LoadModel load,
            bool invertAxles)
        {
            Func<Axle, double> axleFunc = invertAxles
            ? (axle) => { return Y + load.Length - axle.Position; }
            : (axle) => { return Y + axle.Position; };

            var positivePiecesMap = new Dictionary<ProfileYZ, HashSet<Interval>>();
            foreach (var profile in trajectory.Left)
            {
                positivePiecesMap.Add(profile.Value, new HashSet<Interval>());
            }
            foreach (var profile in trajectory.Right)
            {
                positivePiecesMap.Add(profile.Value, new HashSet<Interval>());
            }

            var a = load.Axles.First();
            if (axleFunc(a) - NormConstants.YYY < 0.00001
                && trajectory.Left[a.WheelsDistance.First()].X == NormConstants.XXX)
            {
                //Слева
            }
            if (axleFunc(a) - NormConstants.YYY < 0.00001
                && trajectory.Right[a.WheelsDistance.First()].X == NormConstants.XXX)
            {
                //Справа
            }

            WheelStrain[] wheelStrains;
            wheelStrains = load.Axles.SelectMany(axle =>
                axle.WheelsDistance.SelectMany<double, WheelStrain>(distance =>
                {
                    var leftWheel = GetWheelStrain(trajectory.Left[distance], positivePiecesMap, axle, axleFunc(axle));
                    var rightWheel = GetWheelStrain(trajectory.Right[distance], positivePiecesMap, axle, axleFunc(axle));
                    return [leftWheel, rightWheel];
                })).ToArray();
            var sumStrain = wheelStrains.Sum(x => x.Strain);

            return new VehicleStrain
            {
                SumStrain = sumStrain,
                TotalStrain = sumStrain,
                WheelStrains = wheelStrains,
                IsDirectionForward = !invertAxles,
                PositivePiecesMap = positivePiecesMap,
                Y = Y,
                X = trajectory.X,
            };
        }

        /// <summary>
        /// Рассчет значения поверхности влияния на профиле
        /// </summary>
        /// <param name="positivePieces">позитивные отрезки профиля, на которых искали напряжение. 
        /// Может вернуть null'ы, если рассчёт происходил в отрицательной зоне профиля></param>
        public virtual double GetZValueByY(
            ProfileYZ profile,
            double Y,
            out (Interval? i1, Interval? i2) positivePieces)
        {
            var betweenValues = profile.SortedVectors.FindBetweenValues(Y, (v) => v.X);
            var z = Formulas.GetOrdinat(betweenValues.Left, betweenValues.Right, Y);
            positivePieces = (profile.PositivePieceMap.TryGetValue(betweenValues.Left.X, out Interval? i1) ? i1 : null,
                profile.PositivePieceMap.TryGetValue(betweenValues.Right.X, out Interval? i2) ? i2 : null);
            return z;
        }

        protected virtual WheelStrain GetWheelStrain(ProfileYZ profile, Dictionary<ProfileYZ, HashSet<Interval>> positivePiecesMap, Axle axle, double Y)
        {
            if (profile.MaximumIndexes.Length == 0 || profile.PositivePieceMap.Count() == 0)
            {
                return new WheelStrain
                {
                    Position = new Vector2D
                    {
                        X = profile.X,
                        Y = Y
                    },
                    AxleRef = axle,
                    Strain = 0d,
                    ZValue = 0d,
                };
            }
            ;
            var zValue = GetZValueByY(profile, Y, out (Interval? i1, Interval? i2) positivePieces);
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
    }
}
