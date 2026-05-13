namespace Abdm.Calculation.BLL.Models.Strain
{
    public class VehicleColumnStrainList : List<VehicleColumnStrain>
    {
        public VehicleColumnStrainList() : base() { }

        public VehicleColumnStrainList(IEnumerable<VehicleColumnStrain> collection) : base(collection) { }

        /// <summary>
        /// Итоговое напряжение с учётом коэффициента
        /// </summary>
        public double TotalStrain => this.Sum(x => x.TotalStrain);
    }
}
