using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IBaseVehicleRollingCalculationCoordinator
    {
        Task<ResultExceptionContainer<VehicleRollingBigModel>> PrepareDataModel(
            [DisallowNull] PassTypeCalculationParameters data,
            bool? IsMirroredByZ,
            CancellationToken cancellationToken);

        ResultExceptionContainer<VehicleRollingResult> RollAndGetStrainResult(
            [DisallowNull] VehicleRollingBigModel data,
            CancellationToken cancellationToken);
    }
}