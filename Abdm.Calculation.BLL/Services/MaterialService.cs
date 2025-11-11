using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.DAL.Enums;
using Abdm.Calculation.DAL.Interfaces;
using Mapster;

namespace Abdm.Calculation.BLL.Services
{
    /// <summary>
    /// Сервис получения материала поверхности
    /// </summary>
    public class MaterialService(ISurfaceMaterialRepository surfaceMaterialRepository,
        IPillarMaterialRepository pillarMaterialRepository) : IMaterialService
    {
        /// <summary>
        /// Возвращает абсолютные значения  интервалов для данного иссо
        /// </summary>
        public async Task<IMaterial?> GetMaterial(PassTypeCalculationParameters data,
            CheckPointTypeEnum checkPointType,
            CancellationToken cancellationToken)
        {
            if (checkPointType == CheckPointTypeEnum.Pillar || checkPointType == CheckPointTypeEnum.PillarParts)
            {
                var pillarMaterial = await pillarMaterialRepository.GetPillarMaterial(data.IssoId, data.CheckPointNumber, cancellationToken);
                return pillarMaterial.Adapt<PillarMaterial?>();
            }

            var surfaceMaterial = await surfaceMaterialRepository.GetSurfaceMaterial(data.IssoId, data.CheckPointNumber, cancellationToken);
            return surfaceMaterial.Adapt<SurfaceMaterial?>();
        }
    }
}
