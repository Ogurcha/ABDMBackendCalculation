using Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete;


namespace Abdm.Calculation.BLL.Models.StrainAnalysis.SteelConcrete
{
    public class AnalysisSteelConcrete
    {
        public required SteelConcreteInputParameters InputParameters { get; set; }

        public required SteelConcreteMaterials Materials { get; set; }

        public required SteelConcreteLoads LoadsAtCheckPoint { get; set; }

        public required SectionGeometricCharacteristics SectionGeometric { get; set; }

        public required CreepAccounting CreepAccounting { get; set; }

        //TODO: 6

        //TODO: 7

        public required PermissibleLoadClasses LoadClasses { get; set; }

        public required StressesAtSlabCentroid StressesAtSlab { get; set; }

        //TODO: 10

        public required CorrectionCoefficients Coefficients { get; set; }

        public required SteelBeamBelts SteelBeamBelts { get; set; }

        public required SectionGeometricCharacteristics SectionGeometric13 { get; set; }

        public required AdditionalSectionCharacteristics AdditionalCharacteristics { get; set; }

        public required CreepAccounting CreepAccounting15 { get; set; }

        public required ConcreteStress ShrinkageStress { get; set; }

        public required ConcreteStress TemperatureStress { get; set; }

        public required PermissibleLoadClasses LoadClasses18 { get; set; }

        public required StressesAtSlabCentroid StressesAtSlab19 { get; set; }

        //TODO: 20

        public required CorrectionCoefficients Coefficients21 { get; set; }

        public required SteelBeamBelts SteelBeamBelts22 { get; set; }
    }
}
