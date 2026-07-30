using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.Maths.Helpers
{
    public static class PassTypeFormulas
    {
        /// <summary>
        /// Расстояние от центра ТС до допустимого края интервала
        /// </summary>
        public static double DistanceBetweenIntervalEdgeAndTrajectoryCenter(LoadModel loadModel, IEnumerable<RoadRule> roadRules)
        {
            return Math.Max(loadModel.Width + loadModel.Interval, roadRules.Min(x => x.MinTrajectoryDistance)) / 2;
        }

        /// <summary>
        /// Расстояние от центральной оси ТС до всех возможных его колес С ОДНОЙ СТОРОНЫ
        /// Например, если в параметры передаётся легковушка с двумя <see cref="Axle"/> и четырмя колёсами, 
        /// то вернётся словарь <расстояниеОтКолесаДоЦентра, 2>. Вернётся только одно значение, так как переднее и заднее колесо на одинаковом расстоянии. Число два означает, что на таком расстоянии оба значения. 
        /// </summary>
        /// <returns>возвращает уникальные значения от оси ТС до колеса, количество колёс, проходящих на таком расстоянии и их суммарный вес проходящий на таком расстоянии</returns>
        public static Dictionary<double, (int, double)> DistanceBetweenTrajectoryCenterAndAxles(Axle[] axles)
        {
            return axles.SelectMany(a => a.WheelsDistance.Select(wheelsDistance => (wheelsDistance, a.WheelWeight)))
                .GroupBy(w => w.wheelsDistance).ToDictionary(w => w.Key / 2, w => (w.Count(), w.Sum(w => w.WheelWeight)));
        }

        /// <summary>
        /// Поиск максимального напряжения для одного тс в определенной позиции на выбранной траектории
        /// </summary>
        public static VehicleStrain GetStrainFromVehicleInPosition(VehicleTrajectory trajectory,
            double position,
            VehicleRollingSmallModel data) => data.Load.ActualDirection
                .Select(b => data.Load.Axles
                    .Select(a => a.Position)
                    .Append(data.Load.MassCenterPosition)
                    .Max(relativePosition => data.VehicleStrainProvider!.GetStrainOnTrajectory(
                        trajectory,
                        position - relativePosition,
                        data.Load,
                        !b))
                ).Max()!;

        /// <summary>
        /// Найти центр массы нагрузки
        /// </summary>
        public static double MassCenterPosition(Axle[] axles) => axles.Sum(a => a.Position * a.Weight) / axles.Sum(a => a.Weight);
    }
}
