using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Mapster;

namespace Abdm.Calculation.BLL.Helpers
{
    /// <summary>
    /// Исправляет в коде некорректности бд, которые не исправляются из-за обратной совместимости
    /// </summary>
    public static class DataModelFixer
    {
        private const double AB51Distance = 5.7d;
        private const double AB51Interval = 0.5d;
        private const double AB74Distance = 6d;
        private const double AB74Interval = 0.7d;
        private const double AB151Distance = 7.5d;
        private const double AB151Interval = 0.96d;

        public static VehicleRollingSmallModel Fix(VehicleRollingSmallModel dataModel, SurfaceDataDto surfaceDataDto, PassTypeCalculationParameters rawData)
        {
            dataModel.Surface.Lambda = surfaceDataDto.Lambda;
            if (dataModel.Surface.MyStrength < 0)
            {
                dataModel.Surface.MyStrength = -dataModel.Surface.MyStrength;
                dataModel.Surface.ConstLoad = -dataModel.Surface.ConstLoad;
                dataModel.Surface.PedestrianLoad = -dataModel.Surface.PedestrianLoad;
            }
            if (rawData.LoadSchema.Id == Enums.LoadEnum.AB51)
            {
                dataModel.Load.Distance = AB51Distance;
                dataModel.Load.Interval = AB51Interval;
            }
            if (rawData.LoadSchema.Id == Enums.LoadEnum.AB74)
            {
                dataModel.Load.Distance = AB74Distance;
                dataModel.Load.Interval = AB74Interval;
            }
            if (rawData.LoadSchema.Id == Enums.LoadEnum.AB151)
            {
                dataModel.Load.Distance = AB151Distance;
                dataModel.Load.Interval = AB151Interval;
            }
            dataModel.Load.SecondaryLoadModel = rawData.SecondaryLoadSchema.Adapt<LoadModel>();

            return dataModel;
        }
    }
}
