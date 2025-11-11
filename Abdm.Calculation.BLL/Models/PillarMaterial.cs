using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Модель материала опоры
    /// </summary>
    public class PillarMaterial : IMaterial
    {
        /// <summary>
        /// Материал опоры
        /// </summary>
        public MaterialTypeEnum MaterialType { get; set; }

        /// <summary>
        /// Тип опоры
        /// </summary>
        public PillarTypeEnum PillarType { get; set; }
    }
}
