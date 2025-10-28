using Abdm.Calculation.BLL.Entities;

namespace Abdm.Calculation.BLL.Models.SteelConcrete
{
    /// <summary>
    /// Данные для проверок деформаций железобетонных чекпоинтов
    /// </summary>
    public class SteelConcreteData : IStrainTypeSpecificData
    {
        public required SteelConcreteDataRectangle[] Rectangles { get; set; }

        public required SteelConcreteDataCorner[] Corners { get; set; }
    }
}
