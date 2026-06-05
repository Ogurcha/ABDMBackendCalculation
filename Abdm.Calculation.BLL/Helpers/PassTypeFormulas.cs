using Abdm.Calculation.BLL.Models;

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
            return axles.SelectMany(a => a.WheelsDistance.Select(wheelsDistance => (wheelsDistance, a.wheelWeight)))
                .GroupBy(w => w.wheelsDistance).ToDictionary(w => w.Key / 2, w => (w.Count(), w.Sum(w => w.wheelWeight)));
        }
    }
}
