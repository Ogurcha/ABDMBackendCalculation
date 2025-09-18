using Abdm.Calculation.BLL.Entities;
using Abdm.Calculation.DAL.Entities;
using Mapster;

namespace Abdm.Calculation.BLL.Mappers
{
    public static class MapsterConfig
    {
        public static void MapsterSetup()
        {
            TypeAdapterConfig<PassageInterval, PassageIntervalModel>
            .NewConfig()
            .Map(dst => dst.TotalWidth, src => src.b_gab)
            .Map(dst => dst.SafetyLineLeft, src => src.b_lp)
            .Map(dst => dst.SafetyLineRight, src => src.b_pb);
        }
    }
}
