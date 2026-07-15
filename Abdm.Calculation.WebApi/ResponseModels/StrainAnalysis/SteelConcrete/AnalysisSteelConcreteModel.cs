using System.Text.Json.Serialization;


namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class AnalysisSteelConcreteModel
    {
        [JsonPropertyName("inputParameters1")]
        public SteelConcreteInputParametersModel InputParameters { get; set; }

        [JsonPropertyName("materials2")]
        public SteelConcreteMaterialsModel Materials { get; set; }

        [JsonPropertyName("loadsAtCheckPoint3")]
        public SteelConcreteLoadsModel LoadsAtCheckPoint { get; set; }

        [JsonPropertyName("sectionGeometric4")]
        public SectionGeometricCharacteristicsModel SectionGeometric { get; set; }

        [JsonPropertyName("creepAccounting5")]
        public CreepAccountingModel CreepAccounting { get; set; }

        //TODO: 6

        //TODO: 7

        [JsonPropertyName("loadClasses8")]
        public PermissibleLoadClassesModel LoadClasses { get; set; }

        [JsonPropertyName("stressesAtSlab9")]
        public StressesAtSlabCentroidModel StressesAtSlab { get; set; }

        //TODO: 10

        [JsonPropertyName("coefficients11")]
        public CorrectionCoefficientsModel Coefficients { get; set; }

        [JsonPropertyName("SteelBeamBelts12")]
        public SteelBeamBeltsModel SteelBeamBelts { get; set; }

        [JsonPropertyName("sectionGeometric13")]
        public SectionGeometricCharacteristicsModel SectionGeometric13 { get; set; }

        [JsonPropertyName("AdditionalCharacteristics14")]
        public AdditionalSectionCharacteristicsModel AdditionalCharacteristics { get; set; }

        [JsonPropertyName("creepAccounting15")]
        public CreepAccountingModel CreepAccounting15 { get; set; }

        [JsonPropertyName("shrinkageStress16")]
        public ConcreteStressModel ShrinkageStress { get; set; }

        [JsonPropertyName("temperatureStress17")]
        public ConcreteStressModel TemperatureStress { get; set; }

        [JsonPropertyName("loadClasses18")]
        public PermissibleLoadClassesModel LoadClasses18 { get; set; }

        [JsonPropertyName("stressesAtSlab19")]
        public StressesAtSlabCentroidModel StressesAtSlab19 { get; set; }

        //TODO: 20

        [JsonPropertyName("coefficients21")]
        public CorrectionCoefficientsModel Coefficients21 { get; set; }

        [JsonPropertyName("SteelBeamBelts22")]
        public SteelBeamBeltsModel SteelBeamBelts22 { get; set; }
    }
}
