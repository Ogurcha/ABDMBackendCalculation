using Abdm.Calculation.SteelConcrete.Enums;
using Abdm.Calculation.SteelConcrete.Models;

namespace Abdm.Calculation.SteelConcrete.SteelConcrete
{
    public class Corner : SteelConcreteItem
    {
        public CornerLocationEnum Location { get; set; }

        public double H2 { get; set; }
    }
}
