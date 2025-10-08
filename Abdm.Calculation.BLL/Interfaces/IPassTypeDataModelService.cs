using Abdm.Calculation.BLL.Models.Algorithmic;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.Parameters;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IPassTypeDataModelService
    {
        PassTypeDataModel ComposePassTypeDataModel(PassTypeCalculationParameters inputData, PassageInterval[] passageIntervals, RoadRule[] roadRules);
    }
}