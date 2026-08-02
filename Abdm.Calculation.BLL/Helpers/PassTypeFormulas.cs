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
        /// Например, если в параметры передаётся легковушка с двумя <see cref="Axle"/> и суммарно четырмя колёсами, 
        /// то вернётся словарь <расстояниеОтКолесаДоЦентра, [осьКолесаПереднего, осьКолесаЗаднего]>. 
        /// Вернётся только одно значение, так как переднее и заднее колесо на одинаковом расстоянии.
        /// </summary>
        /// <returns>возвращает уникальные значения от оси ТС до колеса, и оси на таком расстоянии</returns>
        public static Dictionary<double, IGrouping<(double WheelWidth, double WheelWeight), Axle>[]> DistanceBetweenTrajectoryCenterAndAxles(Axle[] axles)
        {
            return axles
                .SelectMany(axle => axle.WheelsDistance.Select(wheelsDistance => (wheelsDistance, axle)))
                .GroupBy(w => w.wheelsDistance)
                .ToDictionary(w => w.Key / 2, w => w.Select(x => x.axle).GroupBy(x => (x.WheelWidth, x.WheelWeight)).ToArray());
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
