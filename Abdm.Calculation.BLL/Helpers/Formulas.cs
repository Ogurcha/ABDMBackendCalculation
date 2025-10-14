using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Helpers
{
    public static class Formulas
    {
        /// <summary>
        /// Расстояние от центра ТС до допустимого края интервала
        /// </summary>
        public static double DistanceBetweenIntervalEdgeAndTrajectoryCenter(LoadModel loadModel, IEnumerable<RoadRule> roadRules)
        {
            return Math.Max(loadModel.Width, roadRules.Min(x => x.MinTrajectoryDistance)) / 2;
        }

        /// <summary>
        /// Расстояние от центральной оси ТС до всех возможных его колес С ОДНОЙ СТОРОНЫ
        /// Например, если в параметры передаётся легковушка с двумя <see cref="Axle"/> и четырмя колёсами, 
        /// то вернётся словарь <расстояниеОтКолесаДоЦентра, 2>. 2, так как переднее и заднее колесо на одинаковом расстоянии.
        /// </summary>
        /// <returns>возвращает уникальные значения от оси ТС до колеса и количество колёс, проходящих на таком расстоянии</returns>
        public static Dictionary<double, int> DistanceBetweenTrajectoryCenterAndAxles(Axle[] axles)
        {
            return axles.SelectMany(a => a.WheelsDistance.Length > 0 ? a.WheelsDistance : NormConstants.DefaultAxleDistance)
                .GroupBy(w => w).ToDictionary(w => w.Key / 2, w => w.Count());
        }
    }
}
