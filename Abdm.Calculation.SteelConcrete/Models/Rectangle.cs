using Abdm.Calculation.SteelConcrete.Enums;

namespace Abdm.Calculation.SteelConcrete.Models
{
    public class Rectangle : SteelConcreteItem
    {
        public double DHeight { get; set; }

        public MaterialEnum Material { get; set; }

        public double Ar { get; set; }

        public double dYr { get; set; }
    }
}
