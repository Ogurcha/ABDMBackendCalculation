using System;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Default;
using Abdm.Calculation.BLL.Models.StrainAnalysis.SteelConcrete;
using Abdm.Calculation.Maths.Models;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis;
using Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default;
using Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete;
using Mapster;

namespace Abdm.Calculation.WebApi.Infrastructure.MapsterConfig
{
    public static class MapsterConfig
    {
        public static void MapsterSetup()
        {
            TypeAdapterConfig<SurfaceDataItemRequestModel, Vector3D>
            .NewConfig()
            .Map(dst => dst.X, src => src.X)
            .Map(dst => dst.Y, src => src.Y)
            .Map(dst => dst.Z, src => src.Z);

            TypeAdapterConfig<AxleRequestModel, Axle>
            .NewConfig()
            .Map(dst => dst.RelativePosition, src => src.Y)
            .Map(dst => dst.WheelWidth, src => src.Wx)
            .Map(dst => dst.WheelLength, src => src.Wy)
            .Map(dst => dst.Weight, src => src.Weight)
            .Map(dst => dst.Position, src => src.AbsY)
            .Map(dst => dst.WheelsDistance, src => src.Wheels);

            TypeAdapterConfig<LoadSchemaRequestModel, LoadSchema>
            .NewConfig()
            .Map(dst => dst.Id, src => src.Id)
            .Map(dst => dst.Type, src => src.TypeId)
            .Map(dst => dst.TypeName, src => src.Type)
            .Map(dst => dst.NameShort, src => src.Name)
            .Map(dst => dst.Width, src => src.Width)
            .Map(dst => dst.Length, src => src.Length)
            .Map(dst => dst.Distance, src => src.Distance)
            .Map(dst => dst.Axles, src => src.Axles)
            .AfterMapping(dst =>
            {
                if (!Enum.IsDefined(dst.Id))
                {
                    dst.Id = LoadEnum.User;
                }
                if (!Enum.IsDefined(dst.Type))
                {
                    dst.Type = LoadGroupTypeEnum.Common;
                }
            }
            );

            TypeAdapterConfig<SurfaceRequestModel, Surface>
            .NewConfig()
            .Map(dst => dst.SurfacePoints, src => src.SurfaceData)
            .Map(dst => dst.PillarData, src => src.LineData)
            .Map(dst => dst.MaxX, src => src.MaxX)
            .Map(dst => dst.MaxY, src => src.MaxY)
            .Map(dst => dst.MaxZ, src => src.MaxZ)
            .Map(dst => dst.MinX, src => src.MinX)
            .Map(dst => dst.MinY, src => src.MinY)
            .Map(dst => dst.MyStrength, src => src.MyStrength == default(int) ? src.SuperStrength : src.MyStrength)
            .Map(dst => dst.ConstLoad, src => src.ConstLoad)
            .Map(dst => dst.PedestrianLoad, src => src.ConstPesh)
            .Map(dst => dst.OtherLoad, src => src.ConstOther)
            .Map(dst => dst.KStrength, src => src.KStrength)
            .Map(dst => dst.WorkSign, src => src.WorkSign)
            .Map(dst => dst.Es, src => src.Es)
            .Map(dst => dst.Ea, src => src.Ea)
            .Map(dst => dst.Eb, src => src.Eb)
            .Map(dst => dst.TetaKr, src => src.TetaKr)
            .Map(dst => dst.EpsilonBetaLim, src => src.EpsilonBetaLim)
            .Map(dst => dst.Rs1, src => src.Rs1)
            .Map(dst => dst.Rs2, src => src.Rs2)
            .Map(dst => dst.Rr, src => src.Rr)
            .Map(dst => dst.Rb, src => src.Rb)
            .Map(dst => dst.Tmax, src => src.Tmax)
            .Map(dst => dst.PlateType, src => src.PlateType)
            .Map(dst => dst.L, src => src.L)
            .Map(dst => dst.Sd, src => src.Sd)
            .Map(dst => dst.SigmaBetaKr, src => src.SigmaBetaKr)
            .Map(dst => dst.SigmaAlfaKr, src => src.SigmaAlfaKr)
            .Map(dst => dst.SigmaBetaShr, src => src.SigmaBetaShr)
            .Map(dst => dst.SigmaAlfaShr, src => src.SigmaAlfaShr)
            .Map(dst => dst.SigmaBetaT, src => src.SigmaBetaT)
            .Map(dst => dst.SigmaAlfaT, src => src.SigmaAlfaT)
            .Map(dst => dst.M1, src => src.M1)
            .Map(dst => dst.M2g, src => src.M2g)
            .Map(dst => dst.Xsi1, src => src.Xsi1)
            .Map(dst => dst.Mp, src => src.Mp);

            TypeAdapterConfig<RoadwayRequestModel, Roadway>
            .NewConfig()
            .Map(dst => dst.LineNumber, src => src.LineNumber ?? 1)
            .Map(dst => dst.RoadHeight, src => src.RoadHeight)
            .Map(dst => dst.LeftSafeline, src => src.LeftSafeline)
            .Map(dst => dst.RightSafeline, src => src.RightSafeline)
            .Map(dst => dst.PositionShift, src => src.PositionShift);

            TypeAdapterConfig<PassTypeCalculationRequest, PassTypeCalculationParameters>
            .NewConfig()
            .Map(dst => dst.IssoId, src => src.CIsso)
            .Map(dst => dst.CheckPointNumber, src => src.Number)
            .Map(dst => dst.LoadId, src => src.CNagruzka)
            .Map(dst => dst.Snip, src => src.Snip)
            .Map(dst => dst.Direction, src => src.Direction)
            .Map(dst => dst.LoadSchema, src => src.LoadSchema)
            .Map(dst => dst.Surface, src => src.Surface)
            .Map(dst => dst.Roadway, src => src.Roadway)
            .Map(dst => dst.SecondaryLoadSchema, src => src.SecondaryLoadSchema);

            TypeAdapterConfig<StrainAnalysisCalculationRequest, StrainAnalysisParameters>
            .NewConfig()
            .Map(dst => dst.IssoId, src => src.CIsso)
            .Map(dst => dst.CheckPointNumber, src => src.Number)
            .Map(dst => dst.LoadId, src => src.CNagruzka)
            .Map(dst => dst.Snip, src => src.Snip)
            .Map(dst => dst.Direction, src => src.Direction)
            .Map(dst => dst.LoadSchema, src => src.LoadSchema)
            .Map(dst => dst.Surface, src => src.Surface)
            .Map(dst => dst.Roadway, src => src.Roadway)
            .Map(dst => dst.SecondaryLoadSchema, src => src.SecondaryLoadSchema);

            TypeAdapterConfig<PassTypeCalculationResult, PassTypeCalculationResponse>
            .NewConfig()
            .Map(dst => dst.CIsso, src => src.IssoId)
            .Map(dst => dst.N, src => src.CPNumber)
            .Map(dst => dst.CNagruzka, src => src.LoadId)
            .Map(dst => dst.Direction, src => src.Direction)
            .Map(dst => dst.Snip, src => src.Snip)
            .Map(dst => dst.PassType, src => src.PassType)
            .Map(dst => dst.Allowed, src => src.Allowed)
            .Map(dst => dst.Intervals, src => src.Intervals);

            TypeAdapterConfig<StrainAnalysisResult, AnalyseStrainCalculationResponse>
            .NewConfig();

            TypeAdapterConfig<AnalysisSummary, AnalysisSummaryModel>
            .NewConfig()
            .Map(dst => dst.StrainCalculationGroupType, src => (int)src.StrainCalculationGroupType)
            .Map(dst => dst.StrainCalculationType, src => (int)src.StrainCalculationType)
            .Map(dst => dst.BarrierInfo, src => src.BarrierInfo)
            .Map(dst => dst.Lambda, src => src.Lambda)
            .Map(dst => dst.Default, src => src.Default)
            .Map(dst => dst.SteelConcrete, src => src.SteelConcrete)
            .Map(dst => dst.MyStrength, src => src.MyStrength)
            .Map(dst => dst.ConstLoad, src => src.ConstLoad)
            .Map(dst => dst.PedestrianLoad, src => src.PedestrianLoad)
            .Map(dst => dst.OtherLoad, src => src.OtherLoad);

            TypeAdapterConfig<AnalysisDefault, AnalysisDefaultModel>
           .NewConfig()
           .Map(dst => dst.HasSafetyLine, src => src.HasSafetyLine)
           .Map(dst => dst.IsForward, src => src.IsForward)
           .Map(dst => dst.Columns, src => src.Columns);

            TypeAdapterConfig<BarrierInfo, BarrierInfoModel>
           .NewConfig()
           .Map(dst => dst.AbsolutePositionFarLeft, src => src.AbsolutePositionFarLeft)
           .Map(dst => dst.AbsolutePositionMiddleLeft, src => src.AbsolutePositionMiddleLeft)
           .Map(dst => dst.AbsolutePositionMiddleRight, src => src.AbsolutePositionMiddleRight)
           .Map(dst => dst.AbsolutePositionFarRight, src => src.AbsolutePositionFarRight)
           .Map(dst => dst.PositionFarLeft, src => src.PositionFarLeft)
           .Map(dst => dst.PositionMiddleLeft, src => src.PositionMiddleLeft)
           .Map(dst => dst.PositionMiddleRight, src => src.PositionMiddleRight)
           .Map(dst => dst.PositionFarRight, src => src.PositionFarRight)
           .Map(dst => dst.HasBarrierInTheMiddle, src => src.HasBarrierInTheMiddle);

            TypeAdapterConfig<AnalysisColumn, AnalysisColumnModel>
            .NewConfig()
            .Map(dst => dst.ColumnNumber, src => src.ColumnNumber)
            .Map(dst => dst.VehicleNumber, src => src.VehicleNumber)
            .Map(dst => dst.PositionX, src => src.PositionX)
            .Map(dst => dst.PositionY, src => src.PositionY)
            .Map(dst => dst.PositionYForImage, src => src.PositionYForImage)
            .Map(dst => dst.Wheels, src => src.Wheels)
            .Map(dst => dst.SumStrain, src => src.SumStrain)
            .Map(dst => dst.TotalStrain, src => src.TotalStrain)
            .Map(dst => dst.Intervals, src => src.Intervals)
            .Map(dst => dst.IntervalProfileVectors, src => src.IntervalProfileVectors)
            .Map(dst => dst.LambdaSmall, src => src.LambdaSmall)
            .Map(dst => dst.Coefficients, src => src.Coefficients);

            TypeAdapterConfig<TrafficJamStrainAnalysis, TrafficJamStrainAnalysisModel>
            .NewConfig()
            .Map(dst => dst.Number, src => src.Number)
            .Map(dst => dst.LeftIntervalStart, src => src.LeftIntervalStart)
            .Map(dst => dst.LeftIntervalEnd, src => src.LeftIntervalEnd)
            .Map(dst => dst.LeftIntervalLength, src => src.LeftIntervalLength)
            .Map(dst => dst.LeftIntervalVolume, src => src.LeftIntervalVolume)
            .Map(dst => dst.LeftIntervalIntensity, src => src.LeftIntervalIntensity)
            .Map(dst => dst.LeftIntervalStrain, src => src.LeftIntervalStrain)
            .Map(dst => dst.RightIntervalStart, src => src.RightIntervalStart)
            .Map(dst => dst.RightIntervalEnd, src => src.RightIntervalEnd)
            .Map(dst => dst.RightIntervalLength, src => src.RightIntervalLength)
            .Map(dst => dst.RightIntervalVolume, src => src.RightIntervalVolume)
            .Map(dst => dst.RightIntervalIntensity, src => src.RightIntervalIntensity)
            .Map(dst => dst.RightIntervalStrain, src => src.RightIntervalStrain)
            .Map(dst => dst.CenterIntervalStart, src => src.CenterIntervalStart)
            .Map(dst => dst.CenterIntervalEnd, src => src.CenterIntervalEnd)
            .Map(dst => dst.CenterIntervalLength, src => src.CenterIntervalLength)
            .Map(dst => dst.SumStrain, src => src.SumStrain);

            TypeAdapterConfig<ProfileVector, ProfileVectorModel>
            .NewConfig()
            .Map(dst => dst.X, src => src.X)
            .Map(dst => dst.Y, src => src.Y);

            TypeAdapterConfig<Coefficients, CoefficientsModel>
            .NewConfig()
            .Map(dst => dst.Stripe, src => src.Stripe)
            .Map(dst => dst.Dynamic, src => src.Dynamic)
            .Map(dst => dst.Reliability, src => src.Reliability)
            .Map(dst => dst.StripeInterval, src => src.StripeInterval)
            .Map(dst => dst.DynamicInterval, src => src.DynamicInterval)
            .Map(dst => dst.ReliabilityInterval, src => src.ReliabilityInterval);

            TypeAdapterConfig<AdditionalSectionCharacteristics, AdditionalSectionCharacteristicsModel>
            .NewConfig()
            .Map(dst => dst.DistanceCombined, src => src.DistanceCombined)
            .Map(dst => dst.DistanceSteel, src => src.DistanceSteel)
            .Map(dst => dst.CombinedSectionStaticMoment, src => src.CombinedSectionStaticMoment)
            .Map(dst => dst.VerticalPlateAreaSteelBeam, src => src.VerticalPlateAreaSteelBeam)
            .Map(dst => dst.HorizontalPlateAreaBottomFlange, src => src.HorizontalPlateAreaBottomFlange)
            .Map(dst => dst.Zb1, src => src.Zb1)
            .Map(dst => dst.S, src => src.S);

            TypeAdapterConfig<AnalysisSteelConcrete, AnalysisSteelConcreteModel>
            .NewConfig()
            .Map(dst => dst.InputParameters, src => src.InputParameters)
            .Map(dst => dst.Materials, src => src.Materials)
            .Map(dst => dst.LoadsAtCheckPoint, src => src.LoadsAtCheckPoint)
            .Map(dst => dst.SectionGeometric, src => src.SectionGeometric)
            .Map(dst => dst.CreepAccounting, src => src.CreepAccounting)
            .Map(dst => dst.LoadClasses, src => src.LoadClasses)
            .Map(dst => dst.StressesAtSlab, src => src.StressesAtSlab)
            .Map(dst => dst.Coefficients, src => src.Coefficients)
            .Map(dst => dst.SteelBeamBelts, src => src.SteelBeamBelts)
            .Map(dst => dst.SectionGeometric13, src => src.SectionGeometric13)
            .Map(dst => dst.AdditionalCharacteristics, src => src.AdditionalCharacteristics)
            .Map(dst => dst.CreepAccounting15, src => src.CreepAccounting15)
            .Map(dst => dst.ShrinkageStress, src => src.ShrinkageStress)
            .Map(dst => dst.TemperatureStress, src => src.TemperatureStress)
            .Map(dst => dst.LoadClasses18, src => src.LoadClasses18)
            .Map(dst => dst.StressesAtSlab19, src => src.StressesAtSlab19)
            .Map(dst => dst.Coefficients21, src => src.Coefficients21)
            .Map(dst => dst.SteelBeamBelts22, src => src.SteelBeamBelts22);

            TypeAdapterConfig<ConcreteStress, ConcreteStressModel>
            .NewConfig()
            .Map(dst => dst.StressInConcrete, src => src.StressInConcrete)
            .Map(dst => dst.StressInArmature, src => src.StressInArmature);

            TypeAdapterConfig<CorrectionCoefficients, CorrectionCoefficientsModel>
            .NewConfig()
            .Map(dst => dst.Theta, src => src.Theta)
            .Map(dst => dst.Ash3, src => src.Ash3)
            .Map(dst => dst.M1, src => src.M1)
            .Map(dst => dst.Ash4, src => src.Ash4);

            TypeAdapterConfig<CreepAccounting, CreepAccountingModel>
            .NewConfig()
            .Map(dst => dst.StressesAtSlab, src => src.StressesAtSlab)
            .Map(dst => dst.ControlValue, src => src.ControlValue)
            .Map(dst => dst.CreepAccountingNotRequired, src => src.CreepAccountingNotRequired)
            .Map(dst => dst.StressesFromConcreteCreepInSlab, src => src.StressesFromConcreteCreepInSlab)
            .Map(dst => dst.StressesFromConcreteCreepInReinforcement, src => src.StressesFromConcreteCreepInReinforcement);

            TypeAdapterConfig<PermissibleLoadClasses, PermissibleLoadClassesModel>
            .NewConfig()
            .Map(dst => dst.PermissibleReferenceTemporaryLoadClasses, src => src.PermissibleReferenceTemporaryLoadClasses)
            .Map(dst => dst.TemporaryLoadsMoments, src => src.TemporaryLoadsMoments)
            .Map(dst => dst.FullBendingMomentSecondStage, src => src.FullBendingMomentSecondStage)
            .Map(dst => dst.FullBendingMoment, src => src.FullBendingMoment);

            TypeAdapterConfig<SectionGeometricCharacteristics, SectionGeometricCharacteristicsModel>
            .NewConfig()
            .Map(dst => dst.SlabAreaReducedToSteelExcludingReinforcement, src => src.SlabAreaReducedToSteelExcludingReinforcement)
            .Map(dst => dst.SlabAreaReducedToSteelWithReinforcement, src => src.SlabAreaReducedToSteelWithReinforcement)
            .Map(dst => dst.SlabStaticMomentReducedToSteelWithReinforcement, src => src.SlabStaticMomentReducedToSteelWithReinforcement)
            .Map(dst => dst.SlabMomentOfInertiaReducedToSteelWithReinforcement, src => src.SlabMomentOfInertiaReducedToSteelWithReinforcement)
            .Map(dst => dst.SlabMomentOfInertiaReducedToSteelAboutCentroidExcludingRebar, src => src.SlabMomentOfInertiaReducedToSteelAboutCentroidExcludingRebar)
            .Map(dst => dst.CompositeBeamSectionArea, src => src.CompositeBeamSectionArea)
            .Map(dst => dst.CompositeBeamStaticMoment, src => src.CompositeBeamStaticMoment)
            .Map(dst => dst.CompositeBeamMomentOfInertia, src => src.CompositeBeamMomentOfInertia)
            .Map(dst => dst.CompositeBeamMomentOfInertiaAboutCentroid, src => src.CompositeBeamMomentOfInertiaAboutCentroid)
            .Map(dst => dst.ConcreteSlabCentroidPosition, src => src.ConcreteSlabCentroidPosition)
            .Map(dst => dst.CompositeSectionCentroidPosition, src => src.CompositeSectionCentroidPosition)
            .Map(dst => dst.DistanceBetweenCompositeAndSlabCentroids, src => src.DistanceBetweenCompositeAndSlabCentroids)
            .Map(dst => dst.DistanceBetweenSteelCentroidAndSlabCentroid, src => src.DistanceBetweenSteelCentroidAndSlabCentroid)
            .Map(dst => dst.DistanceFromCompositeCentroidToTopFiberOfConcreteSlab, src => src.DistanceFromCompositeCentroidToTopFiberOfConcreteSlab)
            .Map(dst => dst.DistanceFromConcreteSlabCentroidToTopFiber, src => src.DistanceFromConcreteSlabCentroidToTopFiber)
            .Map(dst => dst.CombinedSectionMomentOfInertia, src => src.CombinedSectionMomentOfInertia)
            .Map(dst => dst.SectionModulusTop, src => src.SectionModulusTop)
            .Map(dst => dst.SectionModulusBottom, src => src.SectionModulusBottom);

            TypeAdapterConfig<SteelBeamBelt, SteelBeamBeltModel>
            .NewConfig()
            .Map(dst => dst.AK, src => src.AK)
            .Map(dst => dst.StrainAK, src => src.StrainAK)
            .Map(dst => dst.LimitsAK, src => src.LimitsAK)
            .Map(dst => dst.ReserveAK, src => src.ReserveAK)
            .Map(dst => dst.NK, src => src.NK)
            .Map(dst => dst.StrainNK, src => src.StrainNK)
            .Map(dst => dst.LimitsNK, src => src.LimitsNK)
            .Map(dst => dst.ReserveNK, src => src.ReserveNK)
            .Map(dst => dst.N3, src => src.N3)
            .Map(dst => dst.StrainN3, src => src.StrainN3)
            .Map(dst => dst.LimitsN3, src => src.LimitsN3)
            .Map(dst => dst.ReserveN3, src => src.ReserveN3);

            TypeAdapterConfig<SteelBeamBelts, SteelBeamBeltsModel>
            .NewConfig()
            .Map(dst => dst.UpperBelt, src => src.UpperBelt)
            .Map(dst => dst.LowerBelt, src => src.LowerBelt);

            TypeAdapterConfig<SteelConcreteInputParameters, SteelConcreteInputParametersModel>
            .NewConfig()
            .Map(dst => dst.DesignSlabWidth, src => src.DesignSlabWidth)
            .Map(dst => dst.DesignSlabThickness, src => src.DesignSlabThickness)
            .Map(dst => dst.GapTopFlangeToSlabBottom, src => src.GapTopFlangeToSlabBottom)
            .Map(dst => dst.TopFlangePlateThickness, src => src.TopFlangePlateThickness)
            .Map(dst => dst.TopFlangePlateWidth, src => src.TopFlangePlateWidth)
            .Map(dst => dst.WebPlateThickness, src => src.WebPlateThickness)
            .Map(dst => dst.WebPlateWidth, src => src.WebPlateWidth)
            .Map(dst => dst.BottomFlangePlateThickness, src => src.BottomFlangePlateThickness)
            .Map(dst => dst.BottomFlangePlateWidth, src => src.BottomFlangePlateWidth)
            .Map(dst => dst.SteelBeamHeight, src => src.SteelBeamHeight)
            .Map(dst => dst.CompositeSectionHeight, src => src.CompositeSectionHeight)
            .Map(dst => dst.LongitudinalReinforcementArea, src => src.LongitudinalReinforcementArea)
            .Map(dst => dst.LimitedPlasticDeformationFactor, src => src.LimitedPlasticDeformationFactor)
            .Map(dst => dst.SteelSectionArea, src => src.SteelSectionArea)
            .Map(dst => dst.StaticMomentSteelSection, src => src.StaticMomentSteelSection)
            .Map(dst => dst.SteelSectionAreaWithReinforcement, src => src.SteelSectionAreaWithReinforcement)
            .Map(dst => dst.StaticMomentSteelSectionWithReinforcement, src => src.StaticMomentSteelSectionWithReinforcement)
            .Map(dst => dst.MomentOfInertiaSteelSection, src => src.MomentOfInertiaSteelSection)
            .Map(dst => dst.MomentOfInertiaSteelSectionAboutCentroid, src => src.MomentOfInertiaSteelSectionAboutCentroid)
            .Map(dst => dst.SteelSectionCentroidPosition, src => src.SteelSectionCentroidPosition)
            .Map(dst => dst.DistanceFromSteelCentroidToTopFiber, src => src.DistanceFromSteelCentroidToTopFiber)
            .Map(dst => dst.MomentOfInertiaSteelPart, src => src.MomentOfInertiaSteelPart)
            .Map(dst => dst.MomentOfInertiaSteelSectionWithReinforcement, src => src.MomentOfInertiaSteelSectionWithReinforcement)
            .Map(dst => dst.SteelSectionCentroidWithReinforcementPosition, src => src.SteelSectionCentroidWithReinforcementPosition)
            .Map(dst => dst.SectionModulusTopFlangeSteelPart, src => src.SectionModulusTopFlangeSteelPart)
            .Map(dst => dst.SectionModulusBottomFlangeSteelPart, src => src.SectionModulusBottomFlangeSteelPart);

            TypeAdapterConfig<SteelConcreteLoads, SteelConcreteLoadsModel>
            .NewConfig()
            .Map(dst => dst.PermanentLoadsFirstStageMoments, src => src.PermanentLoadsFirstStageMoments)
            .Map(dst => dst.PermanentLoadsSecondStageMoments, src => src.PermanentLoadsSecondStageMoments)
            .Map(dst => dst.PedestrianLoadMoments, src => src.PedestrianLoadMoments);

            TypeAdapterConfig<SteelConcreteMaterials, SteelConcreteMaterialsModel>
            .NewConfig()
            .Map(dst => dst.SteelElasticModulus, src => src.SteelElasticModulus)
            .Map(dst => dst.ReinforcementElasticModulus, src => src.ReinforcementElasticModulus)
            .Map(dst => dst.ConcreteElasticModulus, src => src.ConcreteElasticModulus)
            .Map(dst => dst.ConversionCoefficientFirst, src => src.ConversionCoefficientFirst)
            .Map(dst => dst.ConversionCoefficientSecond, src => src.ConversionCoefficientSecond)
            .Map(dst => dst.ConcreteUltimateCompressiveStrain, src => src.ConcreteUltimateCompressiveStrain)
            .Map(dst => dst.ConcreteElasticModulusForShrinkage, src => src.ConcreteElasticModulusForShrinkage)
            .Map(dst => dst.NbConversionCoefficientFirst, src => src.NbConversionCoefficientFirst)
            .Map(dst => dst.NbConversionCoefficientSecond, src => src.NbConversionCoefficientSecond)
            .Map(dst => dst.ConcreteUltimateStrainForShrinkage, src => src.ConcreteUltimateStrainForShrinkage)
            .Map(dst => dst.UpperSteelDesignStrength, src => src.UpperSteelDesignStrength)
            .Map(dst => dst.LowerSteelDesignStrength, src => src.LowerSteelDesignStrength)
            .Map(dst => dst.SlabReinforcementDesignStrength, src => src.SlabReinforcementDesignStrength)
            .Map(dst => dst.ConcreteDesignStrength, src => src.ConcreteDesignStrength)
            .Map(dst => dst.MaximumTemperatureDifference, src => src.MaximumTemperatureDifference)
            .Map(dst => dst.ConcreteSlabMomentOfInertia, src => src.ConcreteSlabMomentOfInertia)
            .Map(dst => dst.ThinPlateMethodApplicable, src => src.ThinPlateMethodApplicable);

            TypeAdapterConfig<StressesAtSlabCentroid, StressesAtSlabCentroidModel>
            .NewConfig()
            .Map(dst => dst.ConcreteStresses, src => src.ConcreteStresses)
            .Map(dst => dst.ConcreteControlValue, src => src.ConcreteControlValue)
            .Map(dst => dst.ReinforcementStresses, src => src.ReinforcementStresses)
            .Map(dst => dst.ReinforcementControlValue, src => src.ReinforcementControlValue)
            .Map(dst => dst.CalculationCase, src => src.CalculationCase)
            .Map(dst => dst.UnloadingForce, src => src.UnloadingForce)
            .Map(dst => dst.UpperFlangeStresses, src => src.UpperFlangeStresses)
            .Map(dst => dst.UpperFlangeStresses2, src => src.UpperFlangeStresses2)
            .Map(dst => dst.LowerFlangeStresses, src => src.LowerFlangeStresses)
            .Map(dst => dst.LowerFlangeStresses2, src => src.LowerFlangeStresses2);
        }
    }
}
