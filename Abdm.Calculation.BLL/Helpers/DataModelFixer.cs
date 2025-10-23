using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.DAL.Entities;
using Mapster;

namespace Abdm.Calculation.BLL.Helpers
{
    /// <summary>
    /// Исправляет в коде некорректности бд, которые не исправляются из-за обратной совместимости
    /// </summary>
    public static class DataModelFixer
    {
        public static PassTypeSmallModel Fix(PassTypeSmallModel dataModel, SurfaceDataDto surfaceDataDto, PassTypeCalculationParameters rawData)
        {
            dataModel.Surface.Lambda = surfaceDataDto.Lambda;
            if (dataModel.Surface.MyStrength < 0)
            {
                dataModel.Surface.MyStrength = -dataModel.Surface.MyStrength;
                dataModel.Surface.ConstLoad = -dataModel.Surface.ConstLoad;
                dataModel.Surface.PedestrianLoad = -dataModel.Surface.PedestrianLoad;
                dataModel.Surface.IsMirroredByZ = true;
            }
            if (rawData.LoadSchema.Id == Enums.LoadEnum.AB51)
            {
                dataModel.Load.Distance = 5.7d;
                dataModel.Load.Interval = 0.5d;
            }
            if (rawData.LoadSchema.Id == Enums.LoadEnum.AB74)
            {
                dataModel.Load.Distance = 6d;
                dataModel.Load.Interval = 0.7d;
            }
            if (rawData.LoadSchema.Id == Enums.LoadEnum.AB151)
            {
                dataModel.Load.Distance = 7.5d;
                dataModel.Load.Interval = 0.96d;
            }
            dataModel.Load.SecondaryLoadModel = rawData.SecondaryLoadSchema.Adapt<LoadModel>();

            return dataModel;
        }
    }
}
