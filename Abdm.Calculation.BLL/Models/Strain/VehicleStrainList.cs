namespace Abdm.Calculation.BLL.Models.Strain
{
    public class VehicleStrainList : List<VehicleStrain>
    {
        public VehicleStrainList() : base() { }

        public VehicleStrainList(IEnumerable<VehicleStrain> collection) : base(collection) { }

        /// <summary>
        /// Итоговое напряжение с учётом коэффициента
        /// </summary>
        public double TotalStrain => this.Sum(x => x.TotalStrain);
    }
}
