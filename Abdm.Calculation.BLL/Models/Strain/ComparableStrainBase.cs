namespace Abdm.Calculation.BLL.Models.Strain
{
    public abstract class ComparableStrainBase : IComparable<ComparableStrainBase>
    {
        public abstract double TotalStrain { get; set; }

        /// <summary>
        /// Используется в статиских функциях сравнений, сумм, Max и т.д.
        /// </summary>
        public int CompareTo(ComparableStrainBase? other)
        {
            if (other == null) return 1;
            return TotalStrain.CompareTo(other.TotalStrain);
        }
    }
}
