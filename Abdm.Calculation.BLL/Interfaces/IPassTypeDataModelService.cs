using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IPassTypeDataModelService
    {
        PassTypeSmallModel ComposePassTypeDataModel(PassTypeCalculationParameters inputData, PassageInterval[] passageIntervals, RoadRule[] roadRules);
    }
}