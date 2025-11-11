using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Модель материала поверхности
    /// </summary>
    public class SurfaceMaterial : IMaterial
    {
        /// <summary>
        /// Материал поверхности
        /// </summary>
        public MaterialTypeEnum MaterialType { get; set; }

        /// <summary>
        /// Тип статической системы
        /// </summary>
        public StaticSystemTypeEnum StaticSystemType { get; set; }

        /// <summary>
        /// Тип пролётного строения
        /// </summary>
        public SuperStructureTypeEnum SuperStructureType { get; set; }
    }
}
