using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ITrajectoryFilterProvider
    {
        public VehicleTrajectoryFilter[] GetFilters(PassageInterval passageInterval,
           LoadModel load,
           IEnumerable<RoadRule> roadRules);

        public VehicleTrajectoryFilter GetFilter(PassageInterval passageInterval,
            LoadModel load,
            RoadRule roadRule);
    }
}