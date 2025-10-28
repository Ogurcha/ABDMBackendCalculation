using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IPillarDataService
    {
        void UpdateSurfaceDataFromPillarData(SurfaceDataDto? surface, PassageInterval[] passageIntervals);
    }
}