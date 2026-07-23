using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IMaterialService
    {
        Task<IMaterial?> GetMaterial(int issoId,
            int substructureId,
            CheckPointTypeEnum checkPointType,
            CancellationToken cancellationToken);
    }
}