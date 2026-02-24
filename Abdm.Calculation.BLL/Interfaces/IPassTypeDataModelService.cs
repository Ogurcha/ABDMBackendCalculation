using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IPassTypeDataModelService
    {
        VehicleRollingSmallModel ComposePassTypeDataModel(PassTypeCalculationParameters inputData, PassageInterval[] passageIntervals, RoadRule[] roadRules);
    }
}