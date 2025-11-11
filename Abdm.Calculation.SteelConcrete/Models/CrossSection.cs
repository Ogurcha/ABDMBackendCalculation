using Abdm.Calculation.SteelConcrete.SteelConcrete;

namespace Abdm.Calculation.SteelConcrete.Models
{
    /// <summary>
    /// Данные для проверок деформаций железобетонных чекпоинтов
    /// </summary>
    public class CrossSection
    {
        public required Rectangle[] Rectangles { get; set; }

        public required Corner[] Corners { get; set; }
    }

    
}
