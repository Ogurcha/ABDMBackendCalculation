using Abdm.Calculation.BLL.Entities;
using Abdm.Calculation.SteelConcrete.Models;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Данные для проверок деформаций железобетонных чекпоинтов
    /// </summary>
    public class SteelConcreteData : IStrainTypeSpecificData
    {
        public required CrossSection CrossSection { get; set; }

        public IssoSteelConcreteParameters? SteelConcreteParameters { get; set; }
    }
}
