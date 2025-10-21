using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IPillarDataService
    {
        void UpdateSurfaceDataFromPillarData(SurfaceDataDto? surface, PassageInterval[] passageIntervals);
    }
}