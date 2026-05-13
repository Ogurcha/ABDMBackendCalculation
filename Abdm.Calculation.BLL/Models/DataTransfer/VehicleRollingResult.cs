using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Models.DataTransfer
{
    /// <summary>
    /// Результат прокатки Транспортным средством по сооружению
    /// </summary>
    public class VehicleRollingResult
    {
        /// <summary>
        /// Полученные напряжения
        /// </summary>
        public required StrainResult[] StrainResults { get; set; }

        /// <summary>
        /// Замапленные исходные данные
        /// </summary>
        public required VehicleRollingBigModel DataModel { get; set; }
    }
}
