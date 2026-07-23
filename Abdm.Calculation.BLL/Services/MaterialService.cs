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
        public async Task<IMaterial?> GetMaterial(int issoId,
            int substructureId,
            CheckPointTypeEnum checkPointType,
            CancellationToken cancellationToken)
        {
            if (checkPointType == CheckPointTypeEnum.Pillar || checkPointType == CheckPointTypeEnum.PillarParts)
            {
                var pillarMaterial = await pillarMaterialRepository.GetPillarMaterial(issoId, substructureId, cancellationToken);
                return pillarMaterial.Adapt<PillarMaterial?>();
            }

            var surfaceMaterial = await surfaceMaterialRepository.GetSurfaceMaterial(issoId, substructureId, cancellationToken);
            return surfaceMaterial.Adapt<SurfaceMaterial?>();
        }
    }
}
